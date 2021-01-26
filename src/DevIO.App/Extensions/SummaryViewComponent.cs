using DevIO.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevIO.App.Extensions
{
    [ViewComponent(Name = "Sumario")]
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificador _notificador;

        public SummaryViewComponent(INotificador notificador)
        {
            _notificador = notificador;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Possibilitar a compatibilidade de sync para async
            var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());

            // Irá tratar como um erro de uma model as notificações colocadas dentro do notificador
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Mensagem));

            return View();
        }
    }
}
