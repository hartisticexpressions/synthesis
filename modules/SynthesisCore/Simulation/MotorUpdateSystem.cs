using SynthesisAPI.EnvironmentManager;

namespace SynthesisCore.Simulation
{
    public class MotorUpdateSystem : SystemBase
    {
        public override void OnPhysicsUpdate() { }

        public override void OnUpdate()
        {
            foreach (var motorAssemblyManager in EnvironmentManager.GetComponentsWhere<MotorAssemblyManager>(_ => true))
            {
                foreach (var motorAssembly in motorAssemblyManager.AllMotorAssemblies)
                {
                    motorAssembly.Update();
                }
            }
        }

        public override void Setup() { }

        public override void Teardown() { }
    }
}
