using System;
using System.Text;

namespace ScrapingNews.Listener
{
    struct NoticiasQueryString
    {
        public NoticiasQueryString(string texto, DateTime dataInicio, DateTime dataFim, string categoria)
        {
            Texto = texto;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Categoria = categoria;
        }

        public NoticiasQueryString(string texto, DateTime dataInicio, DateTime dataFim)
        {
            Texto = texto;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Categoria = null;
        }

        public short FormSubmitted { get { return 1; } }
        public string Texto { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public string Categoria { get; private set; }
        public short Paginacao { get { return 20; } }

        private string formatarData(DateTime data) => data.ToString("dd/MM/yyyy");

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("?");
            builder.Append($"form.submitted={FormSubmitted}");
            builder.Append($"&texto={Texto}");
            builder.Append($"&dt_inicio={formatarData(DataInicio)}");
            if (DataFim.CompareTo(DateTime.Now) > 0)
                builder.Append($"&dt_fim={formatarData(DataFim)}");
            if (!string.IsNullOrWhiteSpace(Categoria))
                builder.Append($"&categoria={Categoria}");
            builder.Append($"&b_size={Paginacao}");
            return builder.ToString();
        }

    }
}
