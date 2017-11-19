using System.Linq;

namespace IntegrationTests.Simple
{
    public abstract class Spec
    {
        private string _given = string.Empty;
        private string _when = string.Empty;

        protected Spec()
        {
            Given();
            When();
        }

        private void Given()
        {
            var type = this.GetType();
            var methodInfos = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(GivenAttribute), false).Length > 0);
            foreach (var methodInfo in methodInfos)
            {
                methodInfo.Invoke(this, null);
                if (methodInfo.GetCustomAttributes(typeof(GivenAttribute), false).First() is GivenAttribute attribute)
                {
                    _given = attribute.Description;
                }
            }
        }

        private void When()
        {
            var type = this.GetType();
            var methodInfos = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(WhenAttribute), false).Length > 0);
            foreach (var methodInfo in methodInfos)
            {
                methodInfo.Invoke(this, null);
                if (methodInfo.GetCustomAttributes(typeof(WhenAttribute), false).First() is  WhenAttribute attribute)
                {
                    _when = attribute.Description;
                }
            }
        }

        private void UpdateFacts()
        {
            var type = this.GetType();
            var methodInfos = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(ThenAttribute), false).Length > 0);

            // TODO: needs to be done at compile time if we ewant to have this reflected in test output

            foreach (var methodInfo in methodInfos)
            {
                // here we basically change the trait DisplayName with the given when then
                var then = methodInfo.GetCustomAttributes(typeof(ThenAttribute), false).First() as ThenAttribute;
                var displayName = "Given " + _given + ". When" + _when + ". Then " + then.Description; 
                then.DisplayName = displayName;
            }
        }
    }
}
