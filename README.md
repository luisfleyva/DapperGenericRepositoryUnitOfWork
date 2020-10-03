# DapperGenericRepositoryUnitOfWork
## Dapper Generic Repository & UnitOfWork Pattern Implementtion

Based on an existing example on GitHub by Tim Schreiber, you can find the original code [here](https://github.com/timschreiber/DapperUnitOfWork), explained [here](https://iamrufio.com/blog/2017/04/c-unit-of-work-pattern-with-dapper/) 
And on "Generic repository pattern using Dapper" article by Damir Bolic found [here](https://itnext.io/generic-repository-pattern-using-dapper-bd48d9cd7ead)
 
Usage:
```csharp
using DapperGenericUnitOfWorkAndRepository

...

using (var uow = new UnitOfWork("YourConnectionString"))
 {
     var oneBreed = await uow.BreedRepository.FindByNameAsync("oneBreed");
     var oneCat = new Cat { BreedId = oneBreed.Id, Name = "Morris", Age = 12 };
     await uow.CatRepository.AddAsync(oneCat);
     uow.Commit();

     var siamese = new Breed { Name = "Siamese" };
     await uow.BreedRepository.AddAsync(siamese);
     var foo = new Cat { BreedId = siamese.Id, Name = "Foo", Age = 19 };
     var xing = new Cat { BreedId = siamese.Id, Name = "Xing", Age = 6 };
     var xang = new Cat { BreedId = siamese.Id, Name = "Xang", Age = 6 };
     await uow.CatRepository.AddRangeAsync(new List<Cat> { foo, xing, xang });
     uow.Commit();
 }
}
```
