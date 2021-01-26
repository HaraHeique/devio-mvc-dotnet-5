using DevIO.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificador _notificador;

        protected BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected bool IsUsuarioLogado()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
