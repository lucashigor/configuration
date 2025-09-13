using Adasit.Andor.Mapping;
using AdasIt.Andor.Budgets.Domain.Accounts;
using AdasIt.Andor.Budgets.InfrastructureCommands.Entities;
using AdasIt.Andor.TestsUtil;

namespace AdasIt.Andor.Budgets.Tests.InfrastructureCommands
{
    public class AccountMappingTests
    {
        [Fact]
        public void GetValid_MapsAccountEntityToAggregate_Correctly()
        {
            // Arrange
            var dto = GetAccountEntity();

            // Act
            var result = Mappings.GetValid<Account, AccountEntity>(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name.Value);
            Assert.Equal(dto.Description, result.Description.Value);
            Assert.Equal(dto.Status, result.Status.Key);
            Assert.Equal(dto.Currency.Id, result.Currency.Id.Value);
            Assert.Equal(dto.Currency.Name, result.Currency.Name);
            Assert.Equal(dto.Currency.Iso, result.Currency.Iso);
            Assert.Equal(dto.Currency.Symbol, result.Currency.Symbol);

            // Categories
            Assert.Equal(dto._categories.Count, result.Categories.Count);
            foreach (var dtoCategory in dto._categories)
            {
                var mappedCategory = result.Categories.FirstOrDefault(c => c.Id.Value == dtoCategory.Id);
                Assert.NotNull(mappedCategory);
                Assert.Equal(dtoCategory.Name, mappedCategory.Name.Value);
                Assert.Equal(dtoCategory.Description, mappedCategory.Description.Value);
                Assert.Equal(dtoCategory.Type, mappedCategory.Type.Key);

                // SubCategories inside Category
                Assert.Equal(dtoCategory._subCategories.Count, mappedCategory.SubCategories.Count);
                foreach (var dtoSub in dtoCategory._subCategories)
                {
                    var mappedSub = mappedCategory.SubCategories.FirstOrDefault(s => s.Id.Value == dtoSub.Id);
                    Assert.NotNull(mappedSub);
                    Assert.Equal(dtoSub.Name, mappedSub.Name.Value);
                    Assert.Equal(dtoSub.Description, mappedSub.Description.Value);
                    Assert.Equal(dtoSub.CategoryId, mappedSub.CategoryId.Value);
                    Assert.Equal(dtoSub.Type, mappedSub.Type.Key);
                }
            }

            // Payment Methods
            Assert.Equal(dto._paymentMethods.Count, result.PaymentMethods.Count);
            foreach (var dtoPm in dto._paymentMethods)
            {
                var mappedPm = result.PaymentMethods.FirstOrDefault(p => p.Id.Value == dtoPm.Id);
                Assert.NotNull(mappedPm);
                Assert.Equal(dtoPm.Name, mappedPm.Name.Value);
                Assert.Equal(dtoPm.Description, mappedPm.Description.Value);
            }

            // Members / Users
            Assert.Equal(dto._members.Count, result.Members.Count);
            foreach (var dtoUser in dto._members)
            {
                var mappedUser = result.Members.FirstOrDefault(u => u.Id.Value == dtoUser.Id);
                Assert.NotNull(mappedUser);
                Assert.Equal(dtoUser.FirstName, mappedUser.FirstName.Value);
                Assert.Equal(dtoUser.LastName, mappedUser.LastName.Value);
                Assert.Equal(dtoUser.Email, mappedUser.Email.Value);
            }

            // Invites
            Assert.Equal(dto._invites.Count, result.Invites.Count);
            foreach (var dtoInvite in dto._invites)
            {
                var mappedInvite = result.Invites.FirstOrDefault(i => i.Id.Value == dtoInvite.Id);
                Assert.NotNull(mappedInvite);
                Assert.Equal(dtoInvite.Email, mappedInvite.Email.Value);
                Assert.Equal(dtoInvite.Status, mappedInvite.Status.Key);
                Assert.Equal(dtoInvite.InvitingId, mappedInvite.Inviting.Id.Value);
                if (dtoInvite.Guest != null)
                {
                    Assert.Equal(dtoInvite.GuestId, mappedInvite.Guest.Id);
                }
            }
        }

        internal static AccountEntity GetAccountEntity()
        {
            var categoryId1 = Guid.NewGuid();
            var categoryId2 = Guid.NewGuid();

            var currency = GetCurrencyEntity();
            var paymentMethod1 = PaymentMethodEntity(1);
            var paymentMethod2 = PaymentMethodEntity(2);

            var subCategory1 = new List<SubCategoryEntity>
                    {
                        SubCategoryEntity(1, categoryId1, paymentMethod1.Id),
                        SubCategoryEntity(1, categoryId1, paymentMethod1.Id)
                    };

            var subCategory2 = new List<SubCategoryEntity>
                    {
                        SubCategoryEntity(2, categoryId2, paymentMethod2.Id),
                        SubCategoryEntity(2, categoryId2, paymentMethod2.Id)
                    };

            var user1 = GetUserEntity(currency.Id);

            return new AccountEntity
            {
                Name = GeneralFixture.GetValidName(),
                Description = GeneralFixture.GetValidDescription(),
                Currency = currency,
                Status = 1,
                _categories = new List<CategoryEntity>()
                {
                    CategoryEntity(1, subCategory1, categoryId1),
                    CategoryEntity(2, subCategory2, categoryId2)
                },
                _subCategories = subCategory1.Concat(subCategory2).ToList(),
                _paymentMethods = new List<PaymentMethodEntity>
                {
                    PaymentMethodEntity(1),
                    PaymentMethodEntity(2)
                },
                _members = new List<UserEntity>
                {
                    user1
                },
                _invites = new List<InviteEntity>()
                {
                    GetInviteEntity(user1, 1, null),
                    GetInviteEntity(user1, 2, GetUserEntity(currency.Id)),
                    GetInviteEntity(user1, 3, GetUserEntity(currency.Id)),
                }
            };
        }

        internal static CategoryEntity CategoryEntity(
            int type,
            ICollection<SubCategoryEntity> subCategories,
            Guid id)
        {
            return new CategoryEntity
            {
                Id = id,
                Name = GeneralFixture.GetValidName(),
                Description = GeneralFixture.GetValidDescription(),
                StartDate = DateTime.UtcNow,
                DeactivationDate = null,
                Type = type,
                _subCategories = subCategories
            };
        }

        internal static CurrencyEntity GetCurrencyEntity()
        {
            return new CurrencyEntity
            {
                Id = Guid.NewGuid(),
                Name = GeneralFixture.GetValidName(),
                Iso = "USD",
                Symbol = "$"
            };
        }

        internal static FinancialMovementEntity FinancialMovementEntity(
            SubCategoryEntity subCategory,
            int status,
            PaymentMethodEntity paymentMethod,
            decimal value)
        {
            return new FinancialMovementEntity
            {
                Id = Guid.NewGuid(),
                Date = GeneralFixture.Faker.Date.Past(),
                Name = GeneralFixture.GetValidName(),
                SubCategoryId = subCategory.Id,
                SubCategory = subCategory,
                Type = subCategory.Type,
                Status = status,
                PaymentMethodId = paymentMethod.Id,
                PaymentMethod = paymentMethod,
                Value = value
            };
        }

        internal static InviteEntity GetInviteEntity(UserEntity inviting, int status, UserEntity? guest)
        {
            return new InviteEntity
            {
                Id = Guid.NewGuid(),
                Email = GeneralFixture.GetValidEmail(),
                InvitingId = inviting.Id,
                Inviting = inviting,
                GuestId = guest?.Id,
                Guest = guest,
                Status = status
            };
        }

        internal static PaymentMethodEntity PaymentMethodEntity(int type)
        {
            return new PaymentMethodEntity
            {
                Id = Guid.NewGuid(),
                Name = GeneralFixture.GetValidName(),
                Description = GeneralFixture.GetValidDescription(),
                StartDate = DateTime.UtcNow,
                DeactivationDate = null,
                Type = type
            };
        }

        internal static SubCategoryEntity SubCategoryEntity(int type,
            Guid categoryId, Guid defaultPaymentMethodId)
        {
            return new SubCategoryEntity
            {
                Id = Guid.NewGuid(),
                Name = GeneralFixture.GetValidName(),
                Description = GeneralFixture.GetValidDescription(),
                StartDate = GeneralFixture.Faker.Date.Past(),
                DeactivationDate = null,
                CategoryId = categoryId,
                Type = type,
                DefaultPaymentMethodId = defaultPaymentMethodId
            };
        }

        internal static UserEntity GetUserEntity(Guid preferredCurrencyId)
        {
            return new UserEntity
            {
                Id = Guid.NewGuid(),
                FirstName = GeneralFixture.GetValidName(),
                LastName = GeneralFixture.GetValidName(),
                Email = GeneralFixture.GetValidEmail(),
                PreferredCurrencyId = preferredCurrencyId,
                PreferredLanguageId = Guid.NewGuid()
            };
        }
    }
}
