using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ViewModels;
using Prism.Navigation;
using Xunit;

namespace Acme.UnitTests.ViewModels
{
    public class MainPageViewModelTests
    {
        [Fact]
        public void Test1()
        {
            var vm = new MainPageViewModel(null );
            vm.Title = "Test1";
            vm.OnNavigatedTo(new NavigationParameters() { {"Title","Hello"} });
            Assert.Equal(1,1);
        }
    }
}
