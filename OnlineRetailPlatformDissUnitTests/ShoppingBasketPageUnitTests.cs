﻿using Xunit;
using Bunit;
using OnlineRetailPlatformDiss.Pages.Business;
using OnlineRetailPlatformDiss.Pages.Customer;
using OnlineRetailPlatformDiss.Data;
using Microsoft.Extensions.DependencyInjection;
using OnlineRetailPlatformDiss.Services;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace OnlineRetailPlatformDissUnitTests
{
    public class ShoppingBasketPageUnitTests
    {
        [Fact]
        public void ShoppingBasketPageRendersWhenEmpty()
        {
            //Test - Checks that the empty basket message loads when the basket is empty
            //Arrange
            using var ctx = new TestContext();

            //Registering Services.
            ctx.Services.AddSingleton<ApplicationDbContext>(new ApplicationDbContext());
            ctx.Services.AddScoped<AppState>();
            ctx.Services.AddScoped<ShoppingBasketService>();
            ctx.Services.GetRequiredService<NavigationManager>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            //Act
            var cut = ctx.RenderComponent<ShoppingBasket>();

            //Assert
            cut.Markup.Contains("It looks like you haven't added any products to your basket! Please add something!");
        }

        [Fact]
        public void ShoppingBasketRendersWithItemPresent()
        {
            //Test - Checks if the basket displays the BasketProductCard when an item is added...
            //Arrange
            using var ctx = new TestContext();

            //Registering Services
            ctx.Services.AddSingleton<ApplicationDbContext>(new ApplicationDbContext());
            ctx.Services.AddScoped<AppState>();
            ctx.Services.AddScoped<ShoppingBasketService>();
            ctx.Services.GetRequiredService<NavigationManager>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            //Act
            var cutProduct = ctx.RenderComponent<SearchProducts>();
            cutProduct.Find("a:contains('Add to Basket')").Click();

            var cut = ctx.RenderComponent<ShoppingBasket>();

            //Assert - Markup should contain the message when a product is present in basket...
            cut.Markup.Contains("Total Items: 1");
        }

        [Fact]
        public void ShoppingBasketEmptiesBasketWhenEmptyBasketIsPressed()
        {
            //Test - Basket Item returns to zero when Empty Basket is pressed
            //Arrange
            using var ctx = new TestContext();

            //Registering Services
            ctx.Services.AddSingleton<ApplicationDbContext>(new ApplicationDbContext());
            ctx.Services.AddScoped<AppState>();
            ctx.Services.AddScoped<ShoppingBasketService>();
            ctx.Services.GetRequiredService<NavigationManager>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            //Act
            var cutProduct = ctx.RenderComponent<SearchProducts>();
            cutProduct.Find("a:contains('Add to Basket')").Click();

            var cut = ctx.RenderComponent<ShoppingBasket>();

            var aTags = cut.FindAll("a");
            var button = aTags.FirstOrDefault(a => a.ParentElement.TextContent.Contains("Empty Basket"));
            button?.Click();

            var divs = cut.FindAll("div");
            var deleteBtn = divs.FirstOrDefault(a => a.ParentElement.TextContent.Contains("Yes, empty the basket!"));
            deleteBtn?.Click();

            //Assert - Markup should contain the message when basket is empty
            cut.Markup.Contains("Total Items: 0");
        }

        [Fact]
        public void ShoppingBasketCheckoutFailsWhenEmptyBasket()
        {
            //Test - A warning should appear when the checkout button is pressed with an empty basket
            //Arrange
            var ctx = new TestContext();

            //Registering Services
            ctx.Services.AddSingleton<ApplicationDbContext>(new ApplicationDbContext());
            ctx.Services.AddScoped<AppState>();
            ctx.Services.AddScoped<ShoppingBasketService>();
            ctx.Services.GetRequiredService<NavigationManager>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            //Act
            var cut = ctx.RenderComponent<ShoppingBasket>();

            //Find all Buttons with an a tag
            var aTags = cut.FindAll("a");
            var button = aTags.FirstOrDefault(a => a.ParentElement.TextContent.Contains("Checkout"));
            button?.Click();

            //Assert
            cut.Markup.Contains("You need an item in your basket to checkout!");
        }

        [Fact(Skip = "N/A")]
        public void ShoppingBasketCheckoutLoadsWhenProductIsPresent()
        {
            //Test - Checkout loads when button is pressed AND a product is present within the basket
            //Arrange
            using var ctx = new TestContext();

            //Registering Services
            ctx.Services.AddSingleton<ApplicationDbContext>(new ApplicationDbContext());
            ctx.Services.AddScoped<AppState>();
            ctx.Services.AddScoped<ShoppingBasketService>();
            var nav = ctx.Services.GetRequiredService<NavigationManager>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            //Act
            var cutProduct = ctx.RenderComponent<SearchProducts>();
            var a = cutProduct.FindAll("a");
            var btn = a.FirstOrDefault(a => a.ParentElement.TextContent.Contains("Add to Basket"));
            btn?.Click();

            var cut = ctx.RenderComponent<ShoppingBasket>();

            //Find all Buttons with an a tag
            var aTags = cut.FindAll("a");
            var button = aTags.FirstOrDefault(a => a.ParentElement.TextContent.Contains("Checkout"));
            button?.Click(); //Click the Checkout Button

            cut.Markup.Contains("Checkout");

        }
    }
}
