# 🅿️ Controle de Estacionamento

Sistema para controle de entrada, saída e cálculo de tarifas de um estacionamento, com tabela de preços parametrizável por período de vigência.

## 📋 Sobre o projeto

O sistema permite registrar a entrada e saída de veículos (identificados pela placa) e calcular automaticamente o valor a pagar com base na tabela de preços vigente na data de entrada. As regras de cobrança seguem o enunciado do desafio:

- Cobrança de **meia hora** para permanências de até 30 minutos;
- Cobrança da **hora cheia inicial** a partir daí;
- **Horas adicionais** com tolerância de 10 minutos por hora, evitando cobrança indevida de fração excedente pequena;
- Tabela de preços com **vigência por período** (ex.: valores válidos de 01/01/2024 a 31/12/2024), buscada pela data de entrada do veículo.

## 🛠️ Tecnologias utilizadas

- [C# 12 / .NET 8](https://dotnet.microsoft.com/)
- [ASP.NET Core](https://learn.microsoft.com/aspnet/core) — interface web
- [xUnit](https://xunit.net/) — testes unitários
- Arquitetura em camadas (**Domain** / **Web** / **Tests**)
- Desenvolvimento orientado a testes (**TDD**), ciclo Red-Green-Refactor

## 📁 Estrutura da solução

```
Estacionamento.sln
├── src/
│   ├── Estacionamento.Domain/   # Regras de negócio (entidades, cálculo de tarifa)
│   └── Estacionamento.Web/      # Interface web (ASP.NET Core)
└── tests/
    └── Estacionamento.Tests/    # Testes unitários (xUnit)
```

A camada `Domain` concentra as regras de negócio isoladas de qualquer detalhe de infraestrutura ou apresentação, o que permite testá-las de forma unitária e rápida — hoje representadas por `TabelaPreco` (parametrização de valores por vigência) e `CalculadoraTarifa` (cálculo do valor a pagar a partir do tempo de permanência).

## ✅ Status atual

| Funcionalidade | Status |
|---|---|
| Cálculo de tarifa (meia hora, hora cheia, horas adicionais com tolerância) | ✅ Concluído, com testes |
| Registro de entrada/saída de veículos por placa | 🚧 Em desenvolvimento |
| Persistência (armazenamento local) | 🚧 Em desenvolvimento |
| Interface web para operação e parametrização | 🚧 Em desenvolvimento |

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

### Executando a aplicação web

```bash
dotnet run --project src/Estacionamento.Web
```

A aplicação estará disponível em `https://localhost:5001` (ou na porta exibida no terminal).

## 📄 .gitignore

O projeto utiliza o [.gitignore](https://www.toptal.com/developers/gitignore) padrão para projetos .NET/Visual Studio, evitando o versionamento de artefatos de build (`bin/`, `obj/`) e arquivos de usuário/IDE.

---

> This is a challenge by [Coodesh](https://coodesh.com/)
