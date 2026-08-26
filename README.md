# Orchard Core Cookie Consent Module

[![CI](https://github.com/griesoft/griesoft-orchardcore-cookie-consent/actions/workflows/ci.yml/badge.svg)](https://github.com/griesoft/griesoft-orchardcore-cookie-consent/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Griesoft.OrchardCore.CookieConsent.svg)](https://www.nuget.org/packages/Griesoft.OrchardCore.CookieConsent/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A GDPR/TDDDG-compliant cookie consent banner for Orchard Core, built on
[vanilla-cookieconsent](https://github.com/orestbida/cookieconsent) v3 (MIT) by Orest Bida.

> **Status: early development.** The foundation works (banner, categories, script blocking,
> consent-record stub), but the module is not feature-complete. See [docs/PLAN.md](docs/PLAN.md)
> for the roadmap.

## Features

- **Consent banner** rendered on all front-end pages, with opt-in (prior consent) mode.
- **Per-tenant branding**: title, description, layout, position, primary color and logo are
  configurable per tenant in the admin dashboard.
- **Compliance-critical behavior is locked**: opt-in mode, symmetric accept/reject buttons and
  script blocking are fixed by the module and cannot be misconfigured per tenant.
- **Cookie categories**: `necessary` (always on, read-only), `functionality`, `analytics`
  and `marketing` — each optional category can be switched on/off per tenant.
- **Script blocking** until opt-in via `manageScriptTags`, plus automatic clearing of category
  cookies when consent is withdrawn.
- **Consent record logging** (stub): decisions are posted back to the server and written to the
  application log; a durable store is planned.

## Requirements

- .NET 8
- Orchard Core 2.2.1 (the version referenced by this module)

## Installation

Install the NuGet package:

`dotnet add package Griesoft.OrchardCore.CookieConsent`

## Getting started

1. Enable the **Cookie Consent** feature in the Orchard Core admin dashboard.
2. Go to **Configuration → Settings → Cookie Consent** to adjust branding, categories and
   consent logging for the tenant.
3. Mark every non-essential script in your theme or templates so it is blocked until the
   visitor opts in:

```html
<!-- Blocked until the visitor accepts the "analytics" category -->
<script type="text/plain" data-category="analytics" src="https://example.com/analytics.js"></script>

<!-- Inline scripts work the same way -->
<script type="text/plain" data-category="marketing">
    initMarketingPixel();
</script>
```

Scripts without `type="text/plain"` and `data-category` are **not** blocked — auditing and
tagging the tenant's scripts is part of the site setup.

## Configuration

All settings live in the admin UI under **Configuration → Settings → Cookie Consent**:

- **Enable the consent banner** — turn the banner on/off for the tenant.
- **Banner title / description** — custom text, with sensible defaults.
- **Layout / position** — `box`, `cloud` or `bar`; corner or center positions.
- **Primary color / logo URL** — light branding of the banner.
- **Cookie categories** — offer or hide the `functionality`, `analytics` and `marketing`
  categories. `necessary` is always present.
- **Log consent decisions** — post decisions to `/cookieconsent/record` where they are logged.

## Development

### Building

```bash
dotnet build
```

### Testing

```bash
dotnet test
```

## Third-party

This module vendors [vanilla-cookieconsent](https://github.com/orestbida/cookieconsent) 3.1.0
(MIT, © Orest Bida) in `src/wwwroot/vendor/cookieconsent/`.

## License

MIT — see [LICENSE](LICENSE).
