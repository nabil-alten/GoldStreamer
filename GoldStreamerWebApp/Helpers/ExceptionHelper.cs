using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace GoldStreamerWebApp.Helpers
{
    public static class ExceptionHelper
    {
        internal static string ParseErrors(System.Web.Mvc.ModelStateDictionary ModelState)
        {
            StringBuilder errors = new StringBuilder();
            foreach (var value in ModelState.Values)
                if (value.Errors.Count > 0)
                    foreach (var error in value.Errors)
                    {
                        if (string.IsNullOrEmpty(error.ErrorMessage))
                        {
                            Exception innerException = error.Exception;
                            while (innerException != null)
                            {
                                errors.AppendLine(innerException.Message);
                                innerException = innerException.InnerException;
                            }
                        }
                        else
                            errors.AppendLine(error.ErrorMessage);
                    }
            errors.AppendLine(Resources.Messages.SaveFailed);
            return errors.ToString();
        }

    }
}