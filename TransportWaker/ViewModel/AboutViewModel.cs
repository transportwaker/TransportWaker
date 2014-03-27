using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cimbalino.Phone.Toolkit.Services;
using Cimbalino.Phone.Toolkit.Helpers;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TransportWaker.Resources;
using TransportWaker.ViewModel;

namespace TransportWaker.ViewModel
{
    public class AboutViewModel
    {

        private readonly IEmailComposeService _emailComposeService;
        private readonly IMarketplaceReviewService _marketplaceReviewService;
        private readonly IShareLinkService _shareLinkService;
        private readonly string _appUrl;
        private readonly ApplicationManifest _applicationManifest;

        public AboutViewModel(IEmailComposeService emailComposeService,
                               IApplicationManifestService applicationManifestService,
                               IMarketplaceReviewService marketplaceReviewService,
                               IShareLinkService shareLinkService)
        {
            _emailComposeService = emailComposeService;
            _marketplaceReviewService = marketplaceReviewService;
            _shareLinkService = shareLinkService;
            RateCommand = new RelayCommand(this.Rate);
            SendFeedbackCommand = new RelayCommand(this.SendFeedback);
            ShareToMailCommand = new RelayCommand(this.ShareToMail);
            ShareSocialNetworkCommand = new RelayCommand(this.ShareSocialNetwork);
            _applicationManifest = applicationManifestService.GetApplicationManifest();
            _appUrl = string.Concat("http://windowsphone.com/s?appid=", _applicationManifest.App.ProductId);
        }

        public string Author
        {
            get 
            {
                return _applicationManifest.App.Author;
            }
        }

        public string Version
        {
            get 
            {
                return _applicationManifest.App.Version;
            }
        }

        public ICommand RateCommand
        {
            get;
            private set;
        }

        public ICommand SendFeedbackCommand
        {
            get;
            private set;
        }

        public ICommand ShareSocialNetworkCommand
        {
            get;
            private set;
        }

        public ICommand ShareToMailCommand
        {
            get;
            private set;
        }

        private void Rate()
        {
            _marketplaceReviewService.Show();
        }

        private void SendFeedback()
        {
            const string To = "transportwaker@gmail.com";
            const string Subject = "My Feedback";
            var Body = string.Format( "Application {0}\n Version: {1}",
                                      _applicationManifest.App.Title,
                                      _applicationManifest.App.Version);
            _emailComposeService.Show(To, Subject, Body);
        }

        private void ShareSocialNetwork()
        {
            const string Message = "This application is absolutely CRAY CRAY! EVERYBODY GET IT";
            _shareLinkService.Show(_applicationManifest.App.Title, Message, new Uri(_appUrl, UriKind.Absolute));
        }

        private void ShareToMail()
        {
            const string Subject = "Check out TransportWaker App!";
            var Body = string.Concat("This application is absolutely CRAY CRAY! Try it out!", _appUrl);
            _emailComposeService.Show(Subject, Body);
        }
    }
}
