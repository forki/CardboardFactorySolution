namespace Domain.Core.Types

type Name = string

type Result<'T> =
| Success of 'T
| Failure of string
