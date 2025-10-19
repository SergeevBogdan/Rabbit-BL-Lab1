using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
namespace Business_logic___rabbit
{
    public static class ErrorHandlerService
    {
        public static T ExecuteWithHandling<T>(Func<T> action, Func<Exception, T> errorHandler)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                return errorHandler(ex);
            }
        }

        public static void ExecuteWithHandling(Action action, Action<Exception> errorHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                errorHandler(ex);
            }
        }

        public static string HandleRepositoryError(Exception ex)
        {
            return $"Ошибка доступа к данным: {ex.Message}";
        }
    }
}
