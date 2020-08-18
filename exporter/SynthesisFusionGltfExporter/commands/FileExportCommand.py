import adsk.core, adsk.fusion, traceback
import threading
import ctypes
import json
import time
import apper
import pathlib
import random
from apper import AppObjects
from ..gltf.FusionGltfExporter import FusionGltfExporter
from ..gltf.utils.GltfConstants import FileType
class FileExportHandler(adsk.core.CustomEventHandler):
    def __init__(self, command, exporter):
        self.command = command
        self.exporter = exporter
        super().__init__()
    def notify(self, args):
        ui = adsk.core.Application.get().userInterface
        try:
            #eventArgs = json.loads(args.additionalInfo)
 
            
            #ui.messageBox(str(eventArgs['path']))
            #self.ao = AppObjects() +
            home = pathlib.Path.home()
            path =  str(home) + "/AppData/Roaming/Autodesk/Synthesis/Robots/" + str(self.exporter.ao.app.activeDocument.name) + ".glb"
            self.command.on_proccess()
            self.exporter.saveGltf(
                self.exporter.ao.app.activeDocument,
                path,
                FileType.GLB,
                False,
                False,
                False,
                True,
                8,True
            )
            
            #self.command.resume()
        except:
            if ui:
                ui.messageBox('Failed:\n{}'.format(traceback.format_exc()))


class FileCommand:
    """ ## FileCommand

        This is for creating the thread and linking the custom event that connects back to fusion

    """
    handlers = []
    connectID = 'SynthFileWatcher_Export_'+str(random.randint(0,1000))
    def __init__(self):
        self.app = adsk.core.Application.get()
       
        self.ao = AppObjects()
        exporter = FusionGltfExporter(self.ao)
        self.fileExportEvent = self.app.registerCustomEvent(self.connectID)
        onExportCommand = FileExportHandler(self, exporter)
        self.fileExportEvent.add(onExportCommand)

        # self.networkSendEvent = self.app.registerCustomEvent(self.sendID)
        # onNetworkSendSocketCommand = Handlers.NetworkSendSocketHandler()
        # self.networkSendEvent.add(onNetworkSendSocketCommand)

        self.handlers.append(onExportCommand)

        self.stopFlag = threading.Event()
        self.connectThread = FileWatcher(self.stopFlag, self)
       
    def start(self):
        self.connectThread.start()

    def on_proccess(self):
        self.connectThread.clear()
    def end(self):
        self.connectThread.stopWatching()
    def resume(self):
        self.connectThread.resumeWatching()
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
       
        home = pathlib.Path.home()
        self.path =  str(home) + "/AppData/Roaming/Autodesk/Synthesis/watch.synth"
        # fileExportCommand.app.userInterface.messageBox(self.path)
        self.fileExportCommand = fileExportCommand

    def run(self ):
        self.watching = True

        while not self.stopped.wait(1.5): 
            
            with open(self.path, "r+") as f:
                lines = f.readlines()
                self.parseFile(lines)
            #print("Watching...")
    def clear(self):
        open(self.path, "w").close()
    def resumeWatching(self ):
        self.watching = True

    def stopWatching(self ):
        self.watching = False

    def parseFile(self,lines):
    # EXPORT date
    # IMPORT date /path/to/file
        for line in lines:
            sections = line.split()
        
            command = sections[0]
            #path = sections[1]
            #args = {'path': path}
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
