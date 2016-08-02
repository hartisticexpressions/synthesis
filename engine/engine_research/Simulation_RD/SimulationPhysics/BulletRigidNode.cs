﻿using System;
using BulletSharp;
using BulletSharp.SoftBody;
using OpenTK;
using Simulation_RD.Extensions;

namespace Simulation_RD.SimulationPhysics
{
    /// <summary>
    /// Defines a robot joint
    /// </summary>
    class BulletRigidNode : RigidNode_Base
    {
        private static int numHinge = 6;

        /// <summary>
        /// Defines collision mesh.
        /// </summary>
        public CollisionObject BulletObject;
        public Action<float> Update;

        public TypedConstraint joint;

        public BulletRigidNode(Guid guid) : base(guid) { }

        /// <summary>
        /// Creates a Rigid Body from a .bxda file
        /// </summary>
        /// <param name="FilePath"></param>
        public void CreateRigidBody(string FilePath)
        {
            CollisionShape shape;
            WheelDriverMeta wheel = null;
            DefaultMotionState motion;
            BXDAMesh mesh = new BXDAMesh();
            mesh.ReadFromFile(FilePath);
            
            //Is it a wheel?
            if ((wheel = GetSkeletalJoint()?.cDriver?.GetInfo<WheelDriverMeta>()) != null && false) //Later
            {
                shape = new CylinderShapeX(wheel.width, wheel.radius, wheel.radius);
            }

            //Rigid Body Construction
            else
            {
                shape = GetShape(mesh);
            }

            //Current quick fix: scale by 1/4. Please find a better solution.
            motion = new DefaultMotionState(Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(0, 50, 0), Matrix4.CreateTranslation(mesh.physics.centerOfMass.Convert()));
            mesh.physics.mass *= 5;
            RigidBodyConstructionInfo info = new RigidBodyConstructionInfo(mesh.physics.mass, motion, shape, shape.CalculateLocalInertia(mesh.physics.mass));
            info.Friction = 10;
            info.RollingFriction = 10;

            BulletObject = new RigidBody(info);
        }

        /// <summary>
        /// Creates a Soft body from a .bxda file [NOT YET PROPERLY IMPLEMENTED?]
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="worldInfo"></param>
        public void CreateSoftBody(string filePath, SoftBodyWorldInfo worldInfo)
        {
            BXDAMesh mesh = new BXDAMesh();
            mesh.ReadFromFile(filePath);
            
            //Soft body construction
            foreach(BXDAMesh.BXDASubMesh sub in mesh.colliders)
            {
                SoftBody temp = SoftBodyHelpers.CreateFromConvexHull(worldInfo, MeshUtilities.DataToVector(sub.verts));
                temp.WorldTransform += Matrix4.CreateTranslation(0, 10, 0);
                BulletObject = temp;
                temp.Restitution = 0;
            }
        }

        /// <summary>
        /// Creates the joint data
        /// </summary>
        public void CreateJoint()
        {
            if (joint != null || GetSkeletalJoint() == null) // can't have that
                return;
            
            switch (GetSkeletalJoint().GetJointType())
            {
                case SkeletalJointType.ROTATIONAL:
                    RotationalJoint_Base nodeR = (RotationalJoint_Base)GetSkeletalJoint();
                    CollisionObject parentObject = ((BulletRigidNode)GetParent()).BulletObject;
                    WheelDriverMeta wheel = GetSkeletalJoint().cDriver.GetInfo<WheelDriverMeta>();

                    //BasePoint is relative to the child object
                    Matrix4 locJ, locP; //Local Joint Pivot, Local Parent Pivot

                    locJ = Matrix4.CreateTranslation(nodeR.basePoint.Convert());
                    locP = locJ  * BulletObject.WorldTransform.Inverted() * parentObject.WorldTransform;

                    HingeConstraint temp = new HingeConstraint((RigidBody)parentObject, (RigidBody)BulletObject, locP, locJ);
                    joint = temp;
                        
                    if (nodeR.hasAngularLimit)
                        temp.SetLimit(nodeR.angularLimitLow, nodeR.angularLimitHigh);

                    //also need to find a less screwy way to do this
                    Update = (f) => { temp.EnableMotor = true; temp.EnableAngularMotor(true, f, 1000f); };

                    Console.WriteLine("{0} joint made", wheel == null ? "Rotational" : "Wheel");
                    break;
                    
                default:
                    Console.WriteLine("Received joint of type {0}", GetSkeletalJoint().GetJointType());
                    break;
            }            
        }

        /// <summary>
        /// Turns a BXDA mesh into a CompoundShape
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        private static CompoundShape GetShape(BXDAMesh mesh)
        {
            CompoundShape shape = new CompoundShape();

            for (int i = 0; i < mesh.colliders.Count; i++)
            {
                BXDAMesh.BXDASubMesh sub = mesh.colliders[i];
                Vector3[] vertices = MeshUtilities.DataToVector(sub.verts);
                StridingMeshInterface sMesh = MeshUtilities.BulletShapeFromSubMesh(sub, vertices);

                //I don't believe there are any transformations necessary here.
                shape.AddChildShape(Matrix4.Identity, new ConvexTriangleMeshShape(sMesh));
                //Console.WriteLine("Successfully created and added sub shape");                
            }

            return shape;
        }
    }
}
