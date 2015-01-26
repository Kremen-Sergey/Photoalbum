using System.Web;
using System.Web.Mvc;
using PhotoalbumMvcPL.Infrastructure;

namespace PhotoalbumMvcPL.Binders
{
    public class SessionWrapperBinder: IModelBinder
    {
        private const string sessionKey = "SessionWrapperKey";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var sessionWrapper = (SessionWrapper)controllerContext.HttpContext.Session[sessionKey];
            if (sessionWrapper == null)
            {
                sessionWrapper = new SessionWrapper();
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    sessionWrapper.Email = HttpContext.Current.User.Identity.Name;
                controllerContext.HttpContext.Session[sessionKey] = sessionWrapper;
            }
            return sessionWrapper;
        }
    }
}