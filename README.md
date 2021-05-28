# hawk-eyed

Este projeto usa a técnica de raspagem de dados ([web scraping](https://en.wikipedia.org/wiki/Web_scraping)) para recuperar notícias publicadas em um portal.

O sistema deve:
 - recuperar o conteúdo HTML de determinada URL
 - filtrar o conteúdo
 - enviar um e-mail para a pesssoa interessada

O software foi desenvolvido em C# e .net core 3.1

A biblioteca [Html Agility Pack](https://html-agility-pack.net/) foi utilizada para filtrar objetos DOM e recuperar os valores necessários.

O arquivo [packages.txt](https://github.com/gabriels-malta/hawk-eyed/blob/main/packages.txt) possui todos os comandos necessários para a criação da estrutura deste projeto.
