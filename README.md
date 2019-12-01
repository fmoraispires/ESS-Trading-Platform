# ESS-Trading-Platform
[![version][version]]() 
* mestrado integrado em engenharia informática
* Arquitecturas de Software parte I
* ESS Trading Platform WebApp
* francisco morais, pg10293

## Âmbito
O presente documento descreve o desenvolvimento de uma aplicação de negociação que permite investidores e traders abrir, fechar e gerir posições no mercado financeiro, que podem envolver compra e venda de ativos financeiros, para este exemplo commodities e stocks. 

A aplicação suporta a gestão de utilizador, pesquisa de activos na página de mercado (Market), e realizar operações de compra/|venda de posições CFD na página portfolio, e fecho manual. Adicionalmente, pela parametrização dos valores de StopLoss e TakeProfit, as posições CFD são fechadas automáticamente. As posições fechadas são mostradas na página de Histórico, e as operações realizadas de abertura, fecho, e crédito/débitos das operações mostradas na página Operações. As páginas estão todas em inglês.


## Conteúdo
Nesta pasta do GitHub, encontra a seguinte informação:
* Código da aplicação na pasta esstp
* Scripts da base de dados relacional MySQL, na pasta MySQL
* Modelos dos diagramas UML na pasta UML
* Ecrãs da aplicação na pasta Views


## Setup

* 1. Descarregue os scripts MySQL, e no workbench corra primeiro o script "create" e de seguida o script "populate".
* 2. Descarregue a aplicação em csharp, e abra a solução com o visual studio community ou VS Code.
* 3. Compile a aplicação e navegue nos menus.
<img src="https://github.com/fmoraispires/ESS-Trading-Platform/blob/master/esstp/Views/market.png" width="600px">
<img src="https://github.com/fmoraispires/ESS-Trading-Platform/blob/master/esstp/Views/portfolio.png" width="600px">
<img src="https://github.com/fmoraispires/ESS-Trading-Platform/blob/master/esstp/Views/portfolio-history.png" width="600px">
<img src="https://github.com/fmoraispires/ESS-Trading-Platform/blob/master/esstp/Views/operations.png" width="600px">

## Organização do software

O projecto apresenta a seguinte estrutura de ficheiros:

* {Controllers} - definição dos endpoints/routes para os interfaces. São o ponto de entrada para os pedidos http das aplicações cliente.

* {Models/Services} - Contém a lógica de domínio e acesso à base de dados.

* {Models/DataModels} - Representam os dados da aplicação que são guardados na base de dados.

* {Models/ViewModels} - classes de transferência de dados para os controladores exporem uma parte limitada dos dados da base de dados (datamodels) via interfaces, e para o model binding dos pedidos HTTP para os métodos de ação dos controladores.

* {Models/Helpers} - Classes que não se enquadram nas pastas anteriores da camada modelo (models):
DataContext com a classe de acesso à camada de persistência através do Entity Framework Core, que deriva data classe EF Core DbContext e tem propriedades publicas Users para aceder e gerir os dados dos utilizadores; AutoMapper para o mapeamento entre datamodels - viewmodels; AppConfig com os dados de configuração por ficheiro externo à aplicação.

* {Views} - Contém os ficheiros html e cshtml rendered pelas classes Controller. E contém os dados que são passados ou servidos pelos métodos dos controladores para os ficheriso HTML.
As rotas MVC da framework asp.net seguem o padrão /{ControllerName}/{ActionName}.

* {wwwroot} - Contém os ficheiros com informação estática, como ficheiros HTML, Javascript (jQuery para Bootstrap), e CSS.

* {appSettings.json} - É o ficheiro externo que contém os dados de configuração da aplicação, como os dados de ligação à base de dados, e outros parâmetros, como as temporizações do padrão Producer-Consumer.

* {Program.cs} - Contém o ponto de entrada do programa, com a classe Main.

* {Startup.cs} - Contém o código que configura o comportamento da aplicação, como o registo das instâncias dos serviços Singleton ou Scoped.


## Requisitos de Software

|  ID   |            Software             |       Version       |                      Hardware                      |
|:-----:|:-------------------------------:|:-------------------:|:--------------------------------------------------:|
|   1   |              MacOS              |   Mojave 10.14.6    |  1,4 GHz Intel Core i5, 8 GB RAM 2133 MHz   DDR3   |
|   2   |           Dotnet core           |         3.0         |                 512MB RAM, 64-bit                  |
|   3   |              nginx              |       1.17.0        |                         -                          |
|   4   |  MySQL Community Server (GPL)   |       5.7.26        |                         -                          |
|   5   |             Docker              |       1.17.0        |                      18.09.2                       |
|   6   |       Debian GNU/Linux 9        |  4.9.125-linuxkit   |                  (docker images)                   |



<!-- Markdown -->
[version]: https://img.shields.io/badge/version-8.0-brightgreen.svg
