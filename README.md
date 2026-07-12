# 🅿️ Controle de Estacionamento

Sistema web para controle de entrada, saída e cálculo de tarifas de um estacionamento, com tabela de preços parametrizável por período de vigência.

## 📋 Sobre o projeto

O sistema permite registrar a entrada e saída de veículos (identificados pela placa) e calcular automaticamente o valor a pagar com base na tabela de preços vigente na data de entrada. As regras de cobrança seguem o enunciado do desafio:

- Cobrança de **meia hora** para permanências de até 30 minutos;
- Cobrança da **hora cheia inicial** a partir daí;
- **Horas adicionais** com tolerância de 10 minutos por hora, evitando cobrança indevida de fração excedente pequena;
- Tabela de preços com **vigência por período** (ex.: valores válidos de 01/01/2024 a 31/12/2024), buscada pela data de entrada do veículo.

## 🛠️ Tecnologias utilizadas

- [C# 12 / .NET 8](https://dotnet.microsoft.com/)
- [ASP.NET Core MVC](https://learn.microsoft.com/aspnet/core) — interface web
- [Entity Framework Core](https://learn.microsoft.com/ef/core/) + [SQLite](https://www.sqlite.org/) — persistência local
- [xUnit](https://xunit.net/) + [FluentAssertions](https://fluentassertions.com/) — testes automatizados
- Razor Views + Bootstrap — interface
- Arquitetura em camadas (**Domain** / **Web** / **Tests**)
- Desenvolvimento orientado a testes (**TDD**), ciclo Red-Green-Refactor

## 📁 Estrutura da solução

```
Estacionamento.sln
├── src/
│   ├── Estacionamento.Domain/        # Regras de negócio (entidades, cálculo de tarifa)
│   └── Estacionamento.Web/           # Interface web (ASP.NET Core MVC)
│       ├── Controllers/              # EstacionamentoController, TabelasPrecoController, HomeController
│       ├── Data/                     # DbContext e repositórios (EF Core + SQLite)
│       ├── Services/                 # RegistradorDeSaida (orquestração de negócio)
│       ├── Views/                    # Razor Views
│       └── Migrations/               # Migrations do EF Core
└── tests/
    └── Estacionamento.Tests/         # Testes automatizados (xUnit + FluentAssertions)
```

A camada `Domain` concentra as regras de negócio isoladas de qualquer detalhe de infraestrutura ou apresentação, o que permite testá-las de forma unitária e rápida:

- `TabelaPreco` — parametrização de valores por vigência
- `CalculadoraTarifa` — cálculo do valor a pagar a partir do tempo de permanência
- `RegistroEstacionamento` — entrada/saída de um veículo, com suas regras de abertura e fechamento
- `SeletorDeTabelaPreco` — seleção da tabela de preços vigente para uma data

A camada `Web` cuida da persistência (EF Core + SQLite) e da interface (Controllers e Views), orquestrando as classes de domínio através do serviço `RegistradorDeSaida`.

## ✅ Status atual

| Funcionalidade | Status |
|---|---|
| Cálculo de tarifa (meia hora, hora cheia, horas adicionais com tolerância) | ✅ Concluído, com testes |
| Registro de entrada/saída de veículos por placa | ✅ Concluído, com testes |
| Tabela de preços com vigência, parametrizável | ✅ Concluído, com testes |
| Persistência local (EF Core + SQLite) | ✅ Concluído, com testes |
| Interface web para operação e parametrização | ✅ Concluído |

## 🚀 Como instalar e executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Clonando o repositório

```bash
git clone <url-do-repositorio>
cd Projeto-Controle-Estacionamento
```

### Restaurando dependências e compilando

```bash
dotnet restore
dotnet build
```

### Executando os testes

```bash
dotnet test
```

### Aplicando a migration e criando o banco SQLite local

```bash
dotnet tool install --global dotnet-ef --version 8.0.10
dotnet ef database update --project src/Estacionamento.Web --startup-project src/Estacionamento.Web
```

### Executando a aplicação web

```bash
dotnet run --project src/Estacionamento.Web
```

A aplicação estará disponível em `https://localhost:7278` (ou na porta exibida no terminal).

## 🖥️ Como usar

- **Estacionamento** (tela inicial): registrar a entrada de um veículo pela placa, visualizar os veículos com permanência em aberto e marcar a saída (calculando automaticamente o valor a pagar).
- **Tabelas de Preço**: cadastrar novas tabelas de preço, informando o período de vigência, o valor da hora inicial e o valor da hora adicional.

## 📄 .gitignore

O projeto utiliza o [.gitignore](https://www.toptal.com/developers/gitignore) padrão para projetos .NET/Visual Studio, evitando o versionamento de artefatos de build (`bin/`, `obj/`), arquivos de usuário/IDE e o banco de dados local (`*.db`).

---

> This is a challenge by [Coodesh](https://coodesh.com/)
