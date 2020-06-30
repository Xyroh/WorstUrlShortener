# Worst Url Shortener
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-1-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->
Mobile and Desktop apps to interface with a variety of public URL Shortening services

## Status
iOS: [![Build status](https://build.appcenter.ms/v0.1/apps/23951d5c-ad63-41ac-939b-8aec742ad4cf/branches/develop/badge)](https://appcenter.ms)<br/>
Droid: [![Build status](https://build.appcenter.ms/v0.1/apps/767894e1-b6f8-45f6-8fe3-94a478c8394b/branches/develop/badge)](https://appcenter.ms)<br/>
Mac: // TODO<br/>
Windows: // TODO<br/>

## Licence
This is released under the [MIT License](LICENSE) so feel free to take a copy, fork or contribute as you will (see below)

## Usage
First off this is live code, this is the exact codebase that's deployed for a working app in the relevant App Stores, so much of what's included (API Keys etc) is relevant to *my* version, you need to edit it to suit you.

It's written in Xamarin Forms, with a tabbed layout, and a few standard app features of my own in keeping with the 'Worst' Apps brand I use, currently Andoid and iOS and more to follow.  It's a simple throw together app, if it helps you, even if it's just learning Xamarin Forms and putting things together then enjoy.

First point of call is to update your info.plist / AndroidManifest.xml and replace the package names with your own, same with version numbers.

Second step is to edit the SettingsViewModel and replace all those API keys with ones of your own.  This app uses [XyrohLib](https://github.com/Xyroh/XyrohLib) (also open source, but the DLL is incuded) that does lots of things like Crash Reporting (Sentry), Analytics (Appcenter), and bug reports (Freshdesk).  If you don't want those then just blank the keys (and pull out any links to SupportPage in the app.)  If your app sends support tickets to me then I'm not promising to respond 😬

Also in the SettingsViewModel are two parameters for Firebase Dynamic Links (What was Goo.gl) these need replacing and some backend setup for your own version which you can follow the guide [here](https://firebase.google.com/docs/dynamic-links/create-links)  at yery least you will need;

- App package name (Android and iOS)
- iOS Dev account Team ID
- SHA-1 and SHA-256 signing cert fingerprints for Android

I don't blame you if you decide not to use Firebase Links becasue it's not worth the effort, just comment out the Picker option in DefaultPage.xaml and it's gone 🤫

That's about it, play, break it, imporve it and make millions, all up to you.

-- Flish

## Bugs or Feature Requests?
Just create an issue and we'll get back to you

## Contributing
We welcome all contributions and will acknowledge them here, please ensure you have read and agreed to our [code of conduct](CODE_OF_CONDUCT.md) and understood our [contributing guidelines](CONTRIBUTING.md) prior to jumping in

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="http://xyroh.com"><img src="https://avatars3.githubusercontent.com/u/3818800?v=4" width="100px;" alt=""/><br /><sub><b>Andy Flisher</b></sub></a><br /><a href="https://github.com/Xyroh/WorstUrlShortener/commits?author=flish" title="Code">💻</a> <a href="https://github.com/Xyroh/WorstUrlShortener/commits?author=flish" title="Documentation">📖</a> <a href="#projectManagement-flish" title="Project Management">📆</a> <a href="#infra-flish" title="Infrastructure (Hosting, Build-Tools, etc)">🚇</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
