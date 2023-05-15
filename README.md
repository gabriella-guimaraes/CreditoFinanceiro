# CreditoFinanceiro
Projeto de estudos. Desing para um processamento de liberação de crédito para Financiadoras.

# Controle Financeiro API

## Índice

* [1. Resumo do projeto](#1-resumo-do-projeto)
* [2. Endpoints](#2-endpoints)
* [3. Tecnologias utilizadas](#3-tecnologias-utilizadas)
* [4. Inplementações fututras](#4-inplementações-futuras)
* [5. Instalações locais para o Projeto](#4-instalações-locais-para-o-Projeto)


## 1. Resumo de projeto

O projeto Controle Financeiro está sendo desenvolvido para modelar um novo fluxo de liberação de crédito para clientes. A Web API desenvolvida com .NET realiza as seguintes ações:

- Consulta de Financiamentos.
- Registros de novos financiamentos.
- Atualização de informações de financiamentos.
- Remoção de financiamentos da base de dados.


## 2. Endpoints

#### 2.1.1 `GET: api/Financiamentos`

* Consulta a base de dados e retorna todos os registros de financiamentos.
* GET: api/Financiamentos - Consulta os dados de registros por ID.

#### 2.1.2 `PUT: api/Financiamento/5`

* Altera um registro de financiamento.

#### 2.1.3 `POST: api/Financiamento`

* Insere um novo registro de Financiamento dentro da tabela.

Para inserir um novo Financiamento na tabela 'Financiamentos' é necessário informar O ID do Cliente, CPF, Tipo de Financiamento, Valor do Financiamento e a data de vencimento.

A aplicação deverá realizar as seguintes validações:

• O valor máximo a ser liberado para qualquer tipo de empréstimo é de R$ 1.000.000,00;

• A quantidade mínima de parcelas é de 5x e máxima de 72x;

• Para o crédito de pessoa jurídica, o valor mínimo a ser liberado é de R$ 15.000,00;

• A data do primeiro vencimento sempre será no mínimo 15 dias e no máximo 40 dias a partir da data atual. Os resultados precisam conter as seguintes informações:

• Status do crédito (Aprovado ou recusado, de acordo com as premissas acima);

• Valor total com juros (Os juros serão calculados através do incremento da porcentagem de juros no valor do crédito); • Valor dos juros.

Caso o request cumpra as regras de negócio o novo financiamento será registrado no banco de dados.

#### 2.1.4 `DELETE: api/Financiamento`

* Deleta registros da tabela de Financiamentos de acordo com o Id do Financiamento.

## 3. Tecnologias utilizadas

Para desenvolver a _API GG Burguer_ foram utilizadas as seguintes tecnologias e ferramentas:
- Postman;
- .NET;
- EntityFramework
- MySQL
- Trello (organização e planejamento)

## 4. Inplementações futuras

- Interface de usuário (ControleFinanceiro.UI) para interação com BackEnd + Banco de Dados

## 5. Instalações locais para o Projeto

O projeto de Crédito Financeiro roda junto com a Interface. Ambas devem ser inicializadas para que possam ser utilizadas.

#### 5.1.1 `Banco de Dados`

* a aplicação utiliza um banco de dados local para armazenar dados. Os mapeamentos possuem métodos para povoar dados fictícios nas tabelas. Caso prefira, basta comentar os trechos 'builder.HasData(...)' para criar as tabelas vazias e preencher-las via INSERT diretamente na base de dados.

### 5.1.2 `Consultas na base de dados`

* Segue alguns exemplos de consultas que podemos obter apartir da base de dados.

Listar todos os clientes do estado de SP que possuem mais de 60% das parcelas pagas:

SELECT c.*
FROM Clientes c
WHERE c.UF = 'SP' AND (
    SELECT COUNT(*)
    FROM Parcelas p
    INNER JOIN Financiamentos f ON p.FinanciamentoId = f.FinanciamentoId
    WHERE f.ClienteId = c.ClienteId AND p.DataPagamento IS NOT NULL
) / (
    SELECT COUNT(*)
    FROM Parcelas p
    INNER JOIN Financiamentos f ON p.FinanciamentoId = f.FinanciamentoId
    WHERE f.ClienteId = c.ClienteId
) > 0.6;

Listar os primeiros quatro clientes que possuem alguma parcela com mais de cinco dias sem atraso (Data Vencimento maior que data atual E Data Pagamento nula):

SELECT TOP 4 c.*
FROM Clientes c
WHERE EXISTS (
    SELECT *
    FROM Parcelas p
    INNER JOIN Financiamentos f ON p.FinanciamentoId = f.FinanciamentoId
    WHERE f.ClienteId = c.ClienteId AND p.DataVencimento > GETDATE() AND p.DataPagamento IS NULL
);

