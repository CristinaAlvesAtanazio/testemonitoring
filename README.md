# testemonitoring

The project was generated using the [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/CleanArchitecture) version 10.7.0.

## Build

Run `dotnet build` to build the solution.

## Run

To run the application:

```bash
dotnet run --project .\src\AppHost
```

The Aspire dashboard will open automatically, showing the application URLs and logs.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

```bash
dotnet new install Clean.Architecture.Solution.Template::10.7.0
```

## Test

The solution contains unit, integration, and functional tests.

To run the tests:
```bash
dotnet test
```

## Help
To learn more about the template go to the [project website](https://cleanarchitecture.jasontaylor.dev). Here you can find additional guidance, request new features, report a bug, and discuss the template with other users.

---

# 📊 Observabilidade (Prometheus + Grafana)

Este projeto possui monitoramento completo utilizando **OpenTelemetry + Prometheus + Grafana**.

---

## 🧠 Arquitetura

```
.NET API (OpenTelemetry)
        ↓
    Prometheus
        ↓
     Grafana
```

---

## ▶️ Subindo a observabilidade

A aplicação deve estar rodando antes:

```bash
dotnet run --project ./src/AppHost
```

Depois, suba os serviços:

```bash
cd infra
docker compose -f docker-compose.observability.yml up -d
```

---

## 🔎 Acessos

| Serviço     | URL                   |
|------------|-----------------------|
| Prometheus | http://localhost:9090 |
| Grafana    | http://localhost:3000 |

---

## 🔐 Login do Grafana

Usuário: `admin`  
Senha: definida via variável de ambiente

---

## ⚙️ Configurando o Grafana

1. Acesse o Grafana
2. Vá em **Connections > Data Sources**
3. Clique em **Add data source**
4. Selecione **Prometheus**
5. Configure a URL:

```
http://localhost:9090
```

6. Clique em **Save & Test**

---

## 📊 Queries úteis

### Status da aplicação
```promql
up
```

### Taxa de requisições
```promql
rate(http_server_request_duration_seconds_count[1m])
```

### Tempo de resposta
```promql
http_server_request_duration_seconds_sum
```

### GC do .NET
```promql
dotnet_gc_collections_total
```

### Uso de memória
```promql
dotnet_process_memory_working_set_bytes
```

---

## 🔐 Configuração de credenciais

As credenciais do Grafana são definidas via variável de ambiente.

Crie um arquivo `.env` na pasta `infra`:

```env
GRAFANA_PASSWORD=your_password_here
```

⚠️ Este arquivo não deve ser versionado.

---

## 🚀 Observações

- O Prometheus coleta métricas automaticamente via `/metrics`
- O Grafana consome os dados do Prometheus
- A aplicação já está instrumentada com OpenTelemetry

---