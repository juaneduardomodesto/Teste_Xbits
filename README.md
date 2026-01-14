#Purchase System Backend

The system features a complete backend with core business logic, ready for integration with a frontend application. 
The repository structure and code are organized to demonstrate clean architecture, scalability, and integration readiness. 
All backend functionalities are implemented and documented for further development.

# Passo a passo do desafio (fluxo do candidato)

* **Faça um fork** deste repositório.
* **Crie um branch** a partir da `main`:

  ```bash
  git checkout -b feat/{seu-usuario}/crud-produtos-categorias
  ```
* **Implemente o CRUD** conforme especificação (Categorias e Produtos).
* **Instale e configure** seu framework de testes; escreva testes mínimos (happy path).
* **Faça commits pequenos** seguindo *Conventional Commits* (ex.: `feat(produtos): cria endpoint de cadastro`).
* **Abra um Pull Request** para a `main` do seu fork com:

  * Descrição do que foi feito
  * Checklist (CRUD ok, testes passando)
* **Compartilhe o link do PR** para avaliação.

---

## Pacotes já instalados

* **Entity Framework Core** (ORM)
* **FluentValidation** (validações)
* **Swagger/OpenAPI** (documentação e teste rápido)
* **Outros utilitários necessários à API**

> Não incluímos pacotes de teste. O candidato deve escolher e instalar (ex.: **xUnit**, **NUnit** ou **MSTest**) e criar o projeto de testes.

---

## O que será avaliado

* **Corretude do CRUD** (Categorias e Produtos)
* **Regras mínimas**:

  * Categoria: **Nome obrigatório e único**
  * Produto: **Nome obrigatório, Preço > 0, CategoriaId existente**
* **HTTP status corretos** (`201/200/204`, `400/404/409/422`)
* **DTOs e mapeamento manual** (sem expor entidades)
* **Tratamento de erros** padronizado
* **Design de camadas** (Controller → Service → Persistência)
* **Qualidade do código** (legibilidade, **SOLID**, **Clean Code**)
* **Testes** (unitários e/ou integração) com cenários de sucesso e falha
* **Commits** pequenos e descritivos (*Conventional Commits*)
* **Capacidade de debug**: pontos propositalmente incompletos/bugados documentados

---

## Bônus (não obrigatório)

* **Autenticação (JWT)** para endpoints de escrita
* **Paginação** em listagens (`page`, `size`, `total`)
* **Filtros e ordenação** (nome, categoria, faixa de preço)
* * **Seguir o padrão disposto no teste. Clean Architecture.**
*   * **Validações.**

---

## Observações finais
* Deixamos uma **breve estrutura/padrão de pastas** sugestiva do que pode ser feito.
  Fique à vontade para **surpreender** — inclusive iniciando **do zero**, se preferir.

