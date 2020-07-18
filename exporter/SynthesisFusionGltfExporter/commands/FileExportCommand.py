import adsk.core, adsk.fusion, traceback
import threading
import ctypes
import time

class FileExportHandler(adsk.core.CustomEventHandler):
    def __init__(self):
        super().__init__()
    def notify(self, args):
        ui = adsk.core.Application.get().userInterface
        try:
            from ..gltf.GLTFDesignExporter import exportDesign
            ui.messageBox('Exporting')
            exportDesign(True)
        except:
            if ui:
                ui.messageBox('Failed:\n{}'.format(traceback.format_exc()))


class FileCommand:
    """ ## FileCommand

        This is for creating the thread and linking the custom event that connects back to fusion

    """
    handlers = []
    connectID = 'SynthFileWatcher_export'
    def __init__(self):
        self.app = adsk.core.Application.get()
      
        self.fileExportEvent = self.app.registerCustomEvent(self.connectID)
        onExportCommand = FileExportHandler()
        self.fileExportEvent.add(onExportCommand)

        # self.networkSendEvent = self.app.registerCustomEvent(self.sendID)
        # onNetworkSendSocketCommand = Handlers.NetworkSendSocketHandler()
        # self.networkSendEvent.add(onNetworkSendSocketCommand)

        self.handlers.append(onExportCommand)

        self.stopFlag = threading.Event()
        self.connectThread = FileWatcher(self.stopFlag, self)
       
    def start(self):
        self.connectThread.start()

    def deleteMe(self):
        # if (self.handlers.count):
        #    for handler in self.handlers:
        #        self.networkEvent.remove(handler)

        self.app.unregisterCustomEvent(self.connectID)
        # self.app.unregisterCustomEvent(self.sendID)
        self.stopFlag.set()



class FileWatcher(threading.Thread):
    def __init__(self, event, fileExportCommand):
        threading.Thread.__init__(self)
        self.stopped = event
        self.path = "C:/Users/Victo/AppData/Local/Autodesk/Synthesis/watch.synth"
        self.fileExportCommand = fileExportCommand

    def run(self ):
        self.watching = True

        while not self.stopped.wait(1.5): 
            
            with open(self.path, "r+") as f:
                lines = f.readlines()
                self.parseFile(lines)
            #print("Watching...")



    def stopWatching(self ):
        self.watching = False

    def parseFile(self,lines):
    # EXPORT date
    # IMPORT date /path/to/file
        for line in lines:
            sections = line.split()
        
            command = sections[0]

            if 'EXPORT' in command:
                self.fileExportCommand.app.fireCustomEvent(self.fileExportCommand.connectID)



class FileManager(object):
    class FileManager:
        def __init__(self):
            self.FileCommand = FileCommand()
            self.val = None
            self.queue = []

        def start(self):
            self.FileCommand.start()

        def __str__(self):
            return 'self'

        def deleteMe(self):
            self.FileCommand.deleteMe()

    instance = None

    def __new__(cls):
        if not FileManager.instance:
            FileManager.instance = FileManager.FileManager()
        return FileManager.instance

    def __getattr__(self, name):
        return getattr(self.instance, name)

    def __setattr__(self, name):
        return setattr(self.instance, name)

    def start(self):
        FileManager.instance.start()

    def deleteMe(self):
        if FileManager.instance:
            FileManager.instance.deleteMe()
