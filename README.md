# Users API

API REST para gerenciamento de usuários projetada para execução em **containers Docker** e **deploy em Kubernetes (K8s)**.

Este projeto demonstra conceitos fundamentais de **arquitetura cloud-native**, incluindo:

* Containerização de aplicações
* Deploy declarativo com Kubernetes
* Separação de configuração usando ConfigMap
* Gerenciamento de dados sensíveis com Secrets
* Orquestração de containers com Deployment
* Comunicação interna com Services

O objetivo do repositório é servir como **projeto de estudo e portfólio focado em Kubernetes e DevOps**.

---

# Arquitetura da Aplicação

A aplicação segue uma arquitetura comum para workloads Kubernetes.

```
Client
  │
  ▼
Service
  │
  ▼
Deployment
  │
  ▼
Pods (Users API)
  │
  ├── ConfigMap (configuração da aplicação)
  └── Secret (dados sensíveis)
```

---

# Tecnologias Utilizadas

Backend

* .NET

Infraestrutura

* Docker
* Kubernetes
* YAML Manifests
* kubectl

---

# Estrutura do Repositório

```
k8s-users-api
│
├── src/
│   └── UsersAPI
│
├── k8s/
│   ├── configmap.yaml
│   ├── secret.yaml
│   ├── deployment.yaml
│   └── service.yaml
│
├── Dockerfile
├── .dockerignore
└── README.md
```

---

# Pré-requisitos

Para executar o projeto é necessário ter instalado:

* .NET SDK
* Docker
* Kubernetes Cluster
* kubectl

Exemplos de clusters locais:

* Minikube
* Kind
* Docker Desktop Kubernetes

---

# Executando a Aplicação Localmente

Clone o repositório:

```bash
git clone https://github.com/techGabrielBr/k8s-users-api.git
cd k8s-users-api
```

Executar a aplicação:

```bash
dotnet restore
dotnet build
dotnet run --project src/UsersAPI
```

A API ficará disponível em:

```
http://localhost:5000
```

---

# Containerização com Docker

Construir a imagem da aplicação:

```bash
docker build -t users-api .
```

Executar o container:

```bash
docker run -p 5000:5000 users-api
```

A API ficará acessível em:

```
http://localhost:5000
```

---

# Deploy no Kubernetes

Os manifests Kubernetes estão localizados na pasta:

```
k8s/
```

Para aplicar todos os recursos no cluster:

```bash
kubectl apply -f k8s/
```

---

# Verificando os Recursos

Listar recursos criados:

```bash
kubectl get all
```

Ver pods em execução:

```bash
kubectl get pods
```

Ver services:

```bash
kubectl get svc
```

Ver logs de um pod:

```bash
kubectl logs <nome-do-pod>
```

---

# Descrição dos Manifests Kubernetes

## Namespace

Arquivo:

```
k8s/namespace.yaml
```

Cria um namespace dedicado para a aplicação dentro do cluster Kubernetes.

Benefícios:

* isolamento de recursos
* organização do cluster
* separação entre ambientes

---

## ConfigMap

Arquivo:

```
k8s/configmap.yaml
```

Armazena **configurações não sensíveis** da aplicação.

Exemplos de uso:

* variáveis de ambiente
* configurações de aplicação
* parâmetros externos

Isso permite separar **configuração do código da aplicação**.

---

## Secret

Arquivo:

```
k8s/secret.yaml
```

Armazena **dados sensíveis** utilizados pela aplicação.

Exemplos:

* connection strings
* senhas
* tokens
* credenciais de serviços

Secrets são armazenados em **base64** e podem ser injetados nos containers como:

* variáveis de ambiente
* arquivos montados em volume

---

## Deployment

Arquivo:

```
k8s/deployment.yaml
```

Define o Deployment da aplicação.

Responsabilidades:

* criar pods
* manter pods em execução
* recriar pods em caso de falha
* permitir atualização da aplicação

O Deployment define:

* imagem Docker da API
* número de réplicas
* portas do container
* variáveis de ambiente
* integração com ConfigMap e Secret

---

## Service

Arquivo:

```
k8s/service.yaml
```

Cria um Service para expor os pods da aplicação dentro do cluster.

Responsabilidades:

* fornecer um endpoint estável
* balancear requisições entre pods
* permitir comunicação entre serviços

Sem Service, cada pod teria um IP diferente e instável.

---

# Testando a API

Exemplo de requisição:

```bash
curl http://localhost:5000/
```

Criar usuário:

```bash
curl -X POST http://localhost:5000/auth/register \
-H "Content-Type: application/json" \
-d '{
  "name": "Gabriel",
  "email": "gabriel@email.com"
  "senha": "123@teste"
}'
```

---

# Objetivo do Projeto

Este projeto foi criado para estudo de:

* Kubernetes
* Deploy de aplicações containerizadas
* Arquitetura cloud-native
* Práticas de DevOps

Ele demonstra como uma **API .NET pode ser containerizada e executada em um cluster Kubernetes utilizando manifests declarativos**.

---

# Melhorias Futuras

Possíveis melhorias para evolução do projeto:

* CI/CD com GitHub Actions
* publicação automática da imagem Docker
* Ingress para exposição HTTP externa
* Horizontal Pod Autoscaler
* liveness e readiness probes
* resource limits para containers
* Helm Chart para gerenciamento de deploy

---

# Autor

Gabriel

GitHub

https://github.com/techGabrielBr
﻿
