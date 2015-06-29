﻿using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Utility functions for reading/writing BXDJ files
/// </summary>
public class BXDJSkeleton
{
    /// <summary>
    /// Ensures that every node is assigned a model file name by assigning all nodes without a file name a generated name.
    /// </summary>
    /// <param name="baseNode">The base node of the skeleton</param>
    /// <param name="overwrite">Overwrite existing</param>
    public static void SetupFileNames(RigidNode_Base baseNode, bool overwrite = false)
    {
        List<RigidNode_Base> nodes = new List<RigidNode_Base>();
        baseNode.ListAllNodes(nodes);

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].modelFileName == null || overwrite) 
                nodes[i].modelFileName = ("node_" + i + ".bxda");
        }
    }

    /// <summary>
    /// Writes out the skeleton file for the skeleton with the base provided to the path provided.
    /// </summary>
    /// <param name="path">The output file path</param>
    /// <param name="baseNode">The base node of the skeleton</param>
    public static void WriteSkeleton(String path, RigidNode_Base baseNode)
    {
        List<RigidNode_Base> nodes = new List<RigidNode_Base>();
        baseNode.ListAllNodes(nodes);

        // Determine the parent ID for each node in the list.
        int[] parentID = new int[nodes.Count];
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].GetParent() != null)
            {
                parentID[i] = nodes.IndexOf(nodes[i].GetParent());

                if (parentID[i] < 0) throw new Exception("Can't resolve parent ID for " + nodes[i].ToString());
            }
            else
            {
                parentID[i] = -1;
            }
        }

        // Begin IO
        BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create));

        writer.Write(BXDIO.FORMAT_VERSION);

        // Write node values
        writer.Write(nodes.Count);

        for (int i = 0; i < nodes.Count; i++)
        {
            writer.Write(parentID[i]);
            nodes[i].modelFileName = FileUtilities.SanatizeFileName("node_" + i + ".bxda");

            writer.Write(nodes[i].modelFileName);
            writer.Write(nodes[i].GetModelID());
            if (parentID[i] >= 0)
            {
                nodes[i].GetSkeletalJoint().WriteJoint(writer);
            }
        }
        writer.Close();
    }

    /// <summary>
    /// Reads the skeleton contained in the BXDJ file specified and returns the root node for that skeleton.
    /// </summary>
    /// <param name="path">The input BXDJ file</param>
    /// <returns>The root node of the skeleton</returns>
    public static RigidNode_Base ReadSkeleton(string path)
    {
        BinaryReader reader = null;
        try
        {
            reader = new BinaryReader(new FileStream(path, FileMode.Open)); //Throws IOException
            // Sanity check
            uint version = reader.ReadUInt32();
            BXDIO.CheckReadVersion(version); //Throws FormatException

            int nodeCount = reader.ReadInt32();
            if (nodeCount <= 0) throw new Exception("This appears to be an empty skeleton");

            RigidNode_Base root = null;
            RigidNode_Base[] nodes = new RigidNode_Base[nodeCount];

            for (int i = 0; i < nodeCount; i++)
            {
                nodes[i] = RigidNode_Base.NODE_FACTORY();

                int parent = reader.ReadInt32();
                nodes[i].modelFileName = (reader.ReadString());
                nodes[i].modelFullID = (reader.ReadString());

                if (parent != -1)
                {
                    SkeletalJoint_Base joint = SkeletalJoint_Base.ReadJointFully(reader);
                    nodes[parent].AddChild(joint, nodes[i]);
                }
                else
                {
                    root = nodes[i];
                }
            }

            if (root == null)
            {
                throw new Exception("This skeleton has no known base.  \"" + path + "\" is probably corrupted.");
            }

            return root;
        }
        catch (FormatException fe)
        {
            Console.WriteLine("File version mismatch");
            Console.WriteLine(fe);
            return null;
        }
        catch (IOException ie)
        {
            Console.WriteLine("Could not open skeleton file");
            Console.WriteLine(ie);
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        finally
        {
            if (reader != null) reader.Close();
        }
    }

    /// <summary>
    /// Clones joint settings for matching skeletal joints from one skeleton to the other.  This does not overwrite existing joint drivers.
    /// </summary>
    /// <param name="from">Source skeleton</param>
    /// <param name="to">Destination skeleton</param>
    public static void CloneDriversFromTo(RigidNode_Base from, RigidNode_Base to, bool overwrite = false)
    {
        List<RigidNode_Base> tempNodes = new List<RigidNode_Base>();
        from.ListAllNodes(tempNodes);

        Dictionary<string, RigidNode_Base> fromNodes = new Dictionary<string, RigidNode_Base>();
        foreach (RigidNode_Base cpy in tempNodes)
        {
            fromNodes[cpy.GetModelID()] = cpy;
        }

        tempNodes.Clear();
        to.ListAllNodes(tempNodes);
        foreach (RigidNode_Base copyTo in tempNodes)
        {
            RigidNode_Base fromNode;
            if (fromNodes.TryGetValue(copyTo.GetModelID(), out fromNode))
            {
                if (copyTo.GetSkeletalJoint() != null && fromNode.GetSkeletalJoint() != null && copyTo.GetSkeletalJoint().GetJointType() == fromNode.GetSkeletalJoint().GetJointType())
                {
                    if(copyTo.GetSkeletalJoint().cDriver == null || overwrite)
                    {
                        // Swap driver.
                        copyTo.GetSkeletalJoint().cDriver = fromNode.GetSkeletalJoint().cDriver;
                    }

                    if (copyTo.GetSkeletalJoint().attachedSensors.Count == 0 || overwrite)
                    {
                        // Swap sensors.
                        copyTo.GetSkeletalJoint().attachedSensors = fromNode.GetSkeletalJoint().attachedSensors;
                    }
                }
            }
        }
    }

}
