using System;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ScrapingNews.Listener
{
    internal sealed class ScrapAnvisa
    {
        HtmlNodeCollection nodeCollections;
        private readonly string filtroXpath = "//form[@id=\"ultimas-noticias\"]/ul/li";
        private string Filtro;
        private DateTime DataInicio;
        private DateTime DataFim;
        private ScrapAnvisa() { }

        public static ScrapAnvisa ScrapDosUltimosSeteDias(string filtro)
        {
            return new ScrapAnvisa
            {
                Filtro = filtro,
                DataInicio = DateTime.Now.AddDays(-7),
                DataFim = DateTime.Now
            };
        }
        public static ScrapAnvisa ScrapDoUltimoDia(string filtro)
        {
            return new ScrapAnvisa
            {
                Filtro = filtro,
                DataInicio = DateTime.Now,
                DataFim = DateTime.MinValue
            };
        }
        public async Task Carregar()
        {
            HtmlWeb web = new HtmlWeb();

            var queryStr = new NoticiasQueryString(Filtro, DataInicio, DataFim);
            var url = new NoticiasUrl(queryStr);
            var loadWebTask = web.LoadFromWebAsync(url.ToString(), Encoding.UTF8);

            var htmlDOM = await loadWebTask;

            nodeCollections = htmlDOM.DocumentNode.SelectNodes(filtroXpath);
        }

        public HtmlNodeCollection ObterNodes() => this.nodeCollections;

        public (string texto, string inicio, string fim) Filtros() => (Filtro, DataInicio.ToString("dd/MM/yyyy"), DataFim.ToString("dd/MM/yyyy"));

        public (string titulo, string link) ObterHyperlink(HtmlNode node)
        {
            var aLink = node.SelectSingleNode("div/h2/a");
            return (aLink.InnerText.Trim(), aLink.Attributes["href"].Value);
        }

        public string ObterDataDePublicacao(HtmlNode node) => node.SelectSingleNode("div/span/span").InnerText.Split("-")[0].Trim();
    }
}
