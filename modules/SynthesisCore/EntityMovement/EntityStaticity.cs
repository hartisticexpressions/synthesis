using MathNet.Spatial.Euclidean;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.InputManager;
using SynthesisAPI.InputManager.Inputs;
using SynthesisAPI.Utilities;
using SynthesisCore.Systems;

namespace SynthesisCore.EntityMovement
{
    public class EntityStaticity
    {     
        public void SetEntityStaticity(bool isStatic)
        {
            foreach(var selectedRigidBody in EnvironmentManager.GetComponentsWhere<Rigidbody>(_ => true))
            {
                if (isStatic)
                    // Set for static environments but still allow jointed movements
                    selectedRigidBody.IsKinematic = true;
                else
                    // Set for non-static entities
                    selectedRigidBody.IsKinematic = false;
            }
        }
    }
}
