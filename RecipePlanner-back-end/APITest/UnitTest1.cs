using RecipePlanner_back_end.Controllers;

namespace APITest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAPIControllersOnAuthorizatonAccess()
        {
            AuthController authController = new AuthController(null!, null!);

            Assert.AreEqual(authController.ChangeUserName("TestName").Value?.ToString(), "Unauthorized");
            Assert.AreEqual(authController.ChangeEmail("TestEmail").Value?.ToString(), "Unauthorized");
            Assert.AreEqual(authController.ChangeBirthdayDate(DateTime.Now).Value?.ToString(), "Unauthorized");
            Assert.AreEqual(authController.ChangeRegion("TestRegion").Value?.ToString(), "Unauthorized");

            UserRecipeController userRecipeController = new UserRecipeController(null!);

            Assert.AreEqual(userRecipeController.GetUsersRecipies().Value?.ToString(), "Unauthorized");
            Assert.AreEqual(userRecipeController.AddNewRecipe(null!).Value?.ToString(), "Unauthorized");

        }
    }
}