namespace zCustodiaUi.builders
{
    public abstract class TestBuilder
    {
        protected List<Func<Task>> Steps { get; private set; }

        protected TestBuilder()
        {
            Steps = new List<Func<Task>>();
        }

        public TestBuilder AddStep(Func<Task> step)
        {
            Steps.Add(step);
            return this;
        }

        public async Task Execute()
        {
            foreach (var step in Steps)
            {
                await step();
            }
        }

        public async Task Execute(params Func<Task>[] additionalSteps)
        {
            foreach (var step in additionalSteps)
            {
                Steps.Add(step);
            }

            foreach (var step in Steps)
            {
                await step();
            }
        }

        public void ClearSteps()
        {
            Steps.Clear();
        }
    }
}
