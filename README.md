# API Universidade

## Curso

### Um Curso é caracterizado por:
### • Id (do tipo long)
### • Sigla (do tipo string), a sigla do Curso
### • Nome (do tipo string), o nome do Curso

## Aluno

### Um Aluno é caracterizado por:
### • Id (do tipo long)
### • Nome (do tipo string), o nome do Aluno
### • Curso (do tipo Curso), o Curso a que o Aluno está inscrito

## Aluno DTO

### • Id (do tipo long)
### • Nome (do tipo string), o nome do Aluno
### • SiglaCurso (do tipo string), a sigla do Curso a que o Aluno está inscrito

## UnidadeCurricular

### Uma UnidadeCurricular é caracterizada por:
### • Id (do tipo long)
### • Sigla (do tipo string), a sigla da UnidadeCurricular
### • Nome (do tipo string), o nome da UnidadeCurricular
### • Curso (do tipo Curso), o Curso a que pertence a UnidadeCurricular
### • Ano (do tipo int), o ano do Curso em que se insere a UnidadeCurricular

## UnidadeCurricular DTO

### • Id (do tipo long)
### • Sigla (do tipo string), a sigla da UnidadeCurricular
### • Nome (do tipo string), o nome da UnidadeCurricular
### • SiglaCurso (do tipo string), a sigla do Curso a que pertence a UnidadeCurricular
### • Ano (do tipo int), o ano do Curso em que se insere a UnidadeCurricular

## Nota

### • Id (do tipo long)
### • Valor (do tipo double), o valor da Nota obtida, de 0.0 a 20.0
### • UnidadeCurricular (do tipo UnidadeCurricular), a UnidadeCurricular a que diz respeito a Nota
### • Aluno (do tipo Aluno), o Aluno a que pertence a Nota nessa UnidadeCurricular

## Nota DTO

### • Id (do tipo long)
### • Valor (do tipo double), o valor da Nota obtida, de 0.0 a 20.0
### • SiglaUC (do tipo string), a sigla da UnidadeCurricular a que diz respeito a Nota
### • NomeAluno (do tipo string), o nome do Aluno a que pertence a Nota nessa UnidadeCurricular