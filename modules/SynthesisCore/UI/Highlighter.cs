using MathNet.Spatial.Euclidean;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.Utilities;
using SynthesisCore.Meshes;
using SynthesisCore.Systems;

namespace SynthesisCore.UI
{
    public static class Highlighter
    {
        private static Entity jointHighlightEntity;

        public static void HighlightJoint(HingeJoint joint, Entity jointEntity)
        {
            if (!jointHighlightEntity.EntityExists())
            {
                jointHighlightEntity = EnvironmentManager.AddEntity();
                jointHighlightEntity.AddComponent<Transform>();
                jointHighlightEntity.AddComponent<Mesh>();

                // Create mesh
                Mesh m = jointHighlightEntity.GetComponent<Mesh>();
                m.Color = (1f, 1f, 0f, 0.5f);
                Cylinder.Make(m, 16, 0.01, 0.25);
            }

            // Set highlight entity parent
            var parentComponent = jointHighlightEntity.GetComponent<Parent>();
            parentComponent.ParentEntity = jointEntity;

            // Set highlight entity transform
            var jointHighlightTransform = jointHighlightEntity.GetComponent<Transform>();
            jointHighlightTransform.Position = joint.Anchor;

            var axis = joint.Axis.Normalize();
            jointHighlightTransform.Rotation = MathUtil.LookAt(
                axis.IsParallelTo(UnitVector3D.XAxis) ? axis.CrossProduct(UnitVector3D.YAxis) : axis.CrossProduct(UnitVector3D.XAxis),
                axis);

            // Move camera to look at the joint
            var parentPosition = jointEntity.GetComponent<Parent>()?.ParentEntity.GetComponent<Transform>()?.GlobalPosition ?? new Vector3D();
            var cameraOffset = (jointHighlightTransform.GlobalPosition - parentPosition).Normalize().ScaleBy(3);
            CameraController.Instance.cameraTransform.GlobalPosition = jointHighlightTransform.GlobalPosition + cameraOffset;

            CameraController.Instance.cameraTransform.LookAt(jointHighlightTransform.GlobalPosition);
            CameraController.Instance.SetNewFocus(jointHighlightTransform.GlobalPosition, false);
        }

        public static void UnhighlightJoint()
        {
            if (jointHighlightEntity.EntityExists())
                jointHighlightEntity.RemoveEntity();
        }
    }
}
