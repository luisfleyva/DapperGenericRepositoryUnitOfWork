using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DapperGenericRepositoryUnitOfWork;
using DapperGenericRepositoryUnitOfWork.Model;

namespace DapperGenericUnitOfWorkAndRepository.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
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
    }
}
