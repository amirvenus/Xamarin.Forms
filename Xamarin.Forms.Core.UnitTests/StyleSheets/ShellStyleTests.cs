using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.StyleSheets.UnitTests
{
	[TestFixture]
	public class ShellStyleTests : ShellTestBase
	{
		public override void Setup()
		{
			base.Setup();
			Device.PlatformServices = new MockPlatformServices();
			Internals.Registrar.RegisterAll(new Type[0]);
		}

		public override void TearDown()
		{
			base.TearDown();
			Device.PlatformServices = null;
			Application.ClearCurrent();
		}

		[Test]
		public void FlyoutItemDefaultStylesApplied()
		{
			Shell shell = new Shell();
			var shellItem = CreateShellItem();

			shell.Items.Add(shellItem);

			var flyoutItemTemplate = Shell.GetItemTemplate(shellItem);
			var thing = (Element)flyoutItemTemplate.CreateContent();
			thing.Parent = shell;

			var label = thing.LogicalChildren.OfType<Label>().First();
			Assert.AreEqual(TextAlignment.Center, label.VerticalTextAlignment);
			Assert.AreEqual(FontAttributes.Bold, label.FontAttributes);
		}

		[Test]
		public void FlyoutItemLabelStyle()
		{
			var marginThickness = new Thickness(20);
			var classStyle = new Xamarin.Forms.Style(typeof(Label))
			{
				Setters = {
					new Setter { Property = Label.TextColorProperty, Value = Color.Red },
					new Setter { Property = Label.MarginProperty, Value = marginThickness }
				},
				Class = "fooClass",
			};

			Shell shell = new Shell();
			shell.Resources = new ResourceDictionary { classStyle };
			var shellItem = CreateShellItem();

			shellItem.StyleClass = new[] { "fooClass" };
			shell.Items.Add(shellItem);

			var flyoutItemTemplate = Shell.GetItemTemplate(shellItem);
			var thing = (Element)flyoutItemTemplate.CreateContent();
			thing.Parent = shell;

			var label = thing.LogicalChildren.OfType<Label>().First();
			Assert.AreEqual(Color.Red, label.TextColor);
			Assert.AreEqual(marginThickness, label.Margin);
		}
	}
}
