using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Entities;

namespace UnitTests.Fixtures
{

    public static class CourierFixture
    {
        private static readonly Faker<Courier> _faker;

        static CourierFixture()
        {
            _faker = new Faker<Courier>()
                .CustomInstantiator(f => new Courier(
                    f.Person.FullName,
                    f.Company.Cnpj().Replace(".", "").Replace("-", "").Replace("/", ""),
                    f.Date.Past(30),
                    f.Random.String2(12, "0123456789"),
                    f.Random.Enum<CnhType>()
                ))
                .RuleFor(e => e.CnhImage, f => f.Image.PicsumUrl());
        }

        public static Courier Create(CnhType cnhType = CnhType.A)
        {
            var courier = _faker.Generate();
            courier.CnhType = cnhType;
            return courier;
        }

        public static List<Courier> CreateList(int count)
        {
            return _faker.Generate(count);
        }
    }
}
