namespace ScrapingNews.Listener
{
    struct NoticiasUrl
    {
        private readonly string url_ultimas_noticias;
        private readonly NoticiasQueryString _queryString;

        public NoticiasUrl(NoticiasQueryString queryString)
        {
            url_ultimas_noticias = "https://www.gov.br/anvisa/pt-br/assuntos/noticias-anvisa";
            _queryString = queryString;
        }

        public override string ToString() => $"{url_ultimas_noticias}{_queryString.ToString()}";
    }
}
