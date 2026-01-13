# ziGestaoTestsUi

Framework de automação de testes de interface do usuário (UI) para o sistema **ziGestão**, desenvolvido com C# .NET 8.0 e Microsoft Playwright.

## Índice

- [Visão Geral](#visão-geral)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Arquitetura do Projeto](#arquitetura-do-projeto)
- [Fluxo de Execução](#fluxo-de-execução)
- [Estrutura de Diretórios](#estrutura-de-diretórios)
- [Configuração](#configuração)
- [Como Executar](#como-executar)
- [Padrões de Design](#padrões-de-design)
- [Relatórios Allure](#relatórios-allure)

---

## Visão Geral

Este projeto implementa um framework robusto e escalável para automação de testes E2E (End-to-End) da aplicação ziGestão. A arquitetura foi desenhada seguindo princípios de **separação de responsabilidades**, **reutilização de código** e **manutenibilidade**, facilitando a criação e manutenção de casos de teste.

### Características Principais

- ✅ **Page Object Model (POM)**: Abstração da interface do usuário
- ✅ **Builder Pattern**: Orquestração fluente de testes
- ✅ **Data-Driven Tests**: Separação de dados de teste
- ✅ **Video Recording**: Captura automática de vídeos nos testes
- ✅ **Allure Reports**: Relatórios detalhados e interativos
- ✅ **Parallel Execution**: Execução paralela de testes
- ✅ **Environment Variables**: Suporte a múltiplos ambientes

---

## Tecnologias Utilizadas

| Tecnologia | Versão | Finalidade |
|------------|--------|------------|
| **.NET** | 8.0 | Framework de desenvolvimento |
| **C#** | 12 | Linguagem de programação |
| **NUnit** | 3.14.0 | Framework de testes |
| **Microsoft Playwright** | 1.57.0 | Automação de browser |
| **Allure.NUnit** | 2.14.1 | Geração de relatórios |
| **Microsoft.Extensions.Configuration** | 10.0.1 | Gerenciamento de configurações |

---

## Arquitetura do Projeto

A arquitetura adota uma abordagem em camadas com clara separação de responsabilidades:

```
┌─────────────────────────────────────────────────────────────────────┐
│                        LAYER DE TESTES                              │
│                      (tests/)                                       │
│  - Define os cenários de teste                                      │
│  - Aplica anotações de testes (NUnit, Allure)                       │
│  - Orquestra a execução através de Builders                         
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│                      LAYER DE BUILDERS                              │
│                     (builders/)                                     │
│  - Implementa o padrão Builder para fluência                        │
│  - Encadeia ações em sequência lógica                               │
│  - Abstrai a complexidade de orquestração                           │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│                      LAYER DE PÁGINAS                               │
│                       (pages/)                                      │
│  - Implementa o Page Object Model (POM)                             │
│  - Representa páginas da aplicação                                  │
│  - Encapsula interações com elementos                               │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│                      LAYER DE LOCATORS                              │
│                    (locators/)                                      │
│  - Centraliza seletores CSS e XPath                                 │
│  - Facilita manutenção de seletores                                 │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│                      LAYER DE DATA                                  │
│                       (data/)                                       │
│  - Fornece dados para os testes                                     │
│  - Suporta dados de configuração                                    │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│                      LAYER DE UTILS                                 │
│                       (utils/)                                      │
│  - Funções reutilizáveis                                            │
│  - Helpers para interações comuns                                   │
│  - Gerenciamento de vídeos e screenshots                            │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│                      LAYER DE RUNNER                                │
│                      (runner/)                                      │
│  - Configuração inicial do browser (TestBase)                       │
│  - Gerenciamento de ciclo de vida dos testes                        │
│  - Setup e Teardown                                                 │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Fluxo de Execução

O fluxo de execução de um teste segue este padrão:

```
    ┌──────────────────┐
    │   TEST EXECUTION │
    └────────┬─────────┘
             │
             ▼
    ┌──────────────────────────────────────┐
    │ 1. TESTBASE.OneTimeSetUp()           │
    │    - Limpa vídeos antigos            │
    └────────┬─────────────────────────────┘
             │
             ▼
    ┌──────────────────────────────────────┐
    │ 2. TESTBASE.OpenBrowserAsync()       │
    │    ├─ Inicia Playwright              │
    │    ├─ Lança browser Chromium         │
    │    ├─ Configura contexto             │
    │    │  ├─ Viewport (1920x1080)        │
    │    │  ├─ Gravação de vídeo           │
    │    │  └─ Zoom (75%)                  │
    │    ├─ Navega para URL alvo           │
    │    └─ Aguarda carregamento completo  │
    └────────┬─────────────────────────────┘
             │
             ▼
    ┌──────────────────────────────────────┐
    │ 3. TEST CASE EXECUTION               │
    │    ├─ Instancia Page Object          │
    │    ├─ Configura Data (opcional)      │
    │    └─ Executa Builder:               │
    │        ┌──────────────────────────┐  │
    │        │ FeatureBuilder           │  │
    │        │  ├─ .PerformAction()     │  │
    │        │  │  ├─ Write Input       │  │
    │        │  │  ├─ Fill Form         │  │
    │        │  │  └─ Click Button      │  │
    │        │  └─ .ValidateResult()    │  │
    │        │     └─ Verify Outcome    │  │
    │        │  └─ .Execute()           │  │
    │        └──────────────────────────┘  │
    └────────┬─────────────────────────────┘
             │
             ▼
    ┌──────────────────────────────────────┐
    │ 4. TESTBASE.CloseBrowserAsync()      │
    │    ├─ Finaliza gravação de vídeo     │
    │    ├─ Anexa vídeo ao Allure          │
    │    ├─ Fecha contexto do browser      │
    │    ├─ Fecha browser                  │
    │    └─ Dispose do Playwright          │
    └────────┬─────────────────────────────┘
             │
             ▼
    ┌──────────────────┐
    │ TEST COMPLETED   │
    └──────────────────┘
```

---

## Estrutura de Diretórios

```
ziGestaoTestsUi/
│
├── pages/                    # Page Objects (POM)
│   └── [feature]/
│       └── [FeaturePage].cs  # Representação da página da funcionalidade
│
├── locators/                 # Seletores CSS/XPath
│   ├── [feature]/
│   │   └── [FeatureElements].cs  # Seletores da funcionalidade
│   └── GenericElements.cs    # Seletores genéricos reutilizáveis
│
├── builders/                 # Padrão Builder para orquestração
│   ├── [feature]/
│   │   └── [FeatureBuilder].cs  # Builder para fluxos da funcionalidade
│   └── TestBuilder.cs        # Builder base abstrato
│
├── tests/                    # Casos de teste
│   └── [feature]/
│       └── [FeatureTests].cs # Testes da funcionalidade
│
├── data/                     # Dados de teste
│   ├── [feature]/
│   │   └── [FeatureData].cs  # Dados para testes da funcionalidade
│   └── files/                # Arquivos utilizados nos testes
│
├── utils/                    # Utilitários e Helpers
│   ├── Utils.cs              # Métodos reutilizáveis
│   ├── VideoHelper.cs        # Gerenciamento de vídeos
│   ├── ScreenshotHelper.cs   # Capturas de tela
│   └── DataGenerator.cs      # Geradores de dados (CPF, CNPJ)
│
├── runner/                   # Configuração base
│   └── TestBase.cs          # Classe base para testes
│

```


---

## Como Executar

### Pré-requisitos

- .NET 8.0 SDK
- Navegador Chromium (instalado automaticamente pelo Playwright)

### Instalação de Dependências

```bash
dotnet restore
```

### Instalação dos Browsers Playwright

```bash
dotnet tool install --global Microsoft.Playwright.CLI
pwsh bin/Debug/net8.0/playwright.ps1 install
```

### Execução dos Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes em paralelo
dotnet test --parallel

# Executar apenas testes de uma funcionalidade específica
dotnet test --filter "FullyQualifiedName~FeatureTests"

# Executar com output detalhado
dotnet test --logger "console;verbosity=detailed"
```

---

## Padrões de Design

### 1. Page Object Model (POM)

```csharp
// FeaturePage.cs - Encapsula interações com a página da funcionalidade
public class FeaturePage
{
    public async Task PerformAction()
    {
        await _utils.Write(_el.InputField, _data.Value, "write value");
        await _utils.Click(_el.ActionButton, "click action button");
    }
}
```

### 2. Builder Pattern

```csharp
// FeatureTests.cs - Orquestração fluente de testes
await new FeatureBuilder(featurePage)
    .PerformAction()
    .ValidateResult()
    .Execute();
```

### 3. TestBase Pattern

```csharp
// TestBase.cs - Configuração base herdada por todos os testes
public abstract class TestBase
{
    protected async Task<IPage> OpenBrowserAsync() { /* ... */ }
    protected async Task CloseBrowserAsync() { /* ... */ }
}
```

---

## Relatórios Allure

O framework utiliza **Allure** para geração de relatórios detalhados.

### Gerar Relatório

```bash
# Após executar os testes
allure generate allure-results --clean

# Abrir relatório
allure open
```

### Recursos do Relatório

- ✅ Passo a passo das ações executadas
- ✅ Screenshots em caso de falha
- ✅ Vídeos da execução completa
- ✅ Timeline de execução
- ✅ Gráficos e estatísticas
- ✅ Categorização de testes

---

## Desenvolvimento

### Adicionando Novos Testes

1. **Crie o Page Object** em `pages/nova-feature/`
2. **Defina os Locators** em `locators/nova-feature/`
3. **Crie o Builder** em `builders/nova-feature/`
4. **Implemente os Testes** em `tests/nova-feature/`

### Boas Práticas

- Mantenha os seletores centralizados na pasta `locators/`
- Utilize os métodos reutilizáveis de `Utils.cs`
- Documente cada passo com AllureStep attributes
- Use nomes descritivos para testes e métodos
- Separe dados de teste da lógica de teste

---

## Licença

© 2024 zCustodia. Todos os direitos reservados.
