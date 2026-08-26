# Griesoft.OrchardCore.CookieConsent — Architecture & Roadmap

## Goal

A GDPR/TDDDG-compliant, brandable cookie consent solution for multi-tenant Orchard Core
hosting, sellable as a paid add-on per tenant. Built on
[vanilla-cookieconsent](https://github.com/orestbida/cookieconsent) v3 (MIT, vendored).

## Architecture

### In-tenant module (this repo)

```
Orchard Core tenant
 ├─ CookieConsentFilter (result filter)      injects assets + per-tenant config into front-end pages
 ├─ CookieConsentSettings (ISite section)    per-tenant look & feel, category toggles, logging switch
 ├─ CookieConsentSettingsDisplayDriver       admin UI (Configuration → Settings → Cookie Consent)
 ├─ ConsentController (/cookieconsent/record) receives consent decisions from the banner
 ├─ IConsentRecordService                    consent record store (currently: log-only stub)
 └─ wwwroot
     ├─ vendor/cookieconsent/                vendored vanilla-cookieconsent 3.1.0 (js/css, MIT)
     └─ scripts/cookieconsent-init.js        locked banner bootstrap (opt-in, symmetric buttons,
                                             script blocking); reads window.griesoftCookieConsent
```

**Design principle — locked compliance, configurable branding.** Tenants may change colors,
logo, text, layout and position. They may NOT change opt-in mode, symmetric accept/reject,
script blocking, or the presence of the "necessary" category. Those are templated in
`cookieconsent-init.js` and are not part of the settings surface. This keeps every tenant's
banner defensible without per-tenant legal review of banner mechanics.

### Off-host scanner service (Phase 2, separate repo/deployment)

Automatic cookie scanning requires a headless browser, which must NOT run on the shared
tenant host (CPU/memory spikes, attack surface, noisy-neighbor risk). It will be a standalone
service (Azure Function or Container App) that:

1. crawls a tenant's public pages with a headless browser,
2. observes cookies/storage set before and after simulated consent,
3. categorizes them against a maintained cookie database,
4. pushes the resulting declaration back to the tenant via an authenticated API in this module.

## Roadmap

### Phase 1 — MVP (in-tenant, this module)

- [x] Foundation: module scaffold, settings section, admin editor, banner injection,
      vendored engine, script-blocking hook, consent-record stub (this repo, v0.1).
- [ ] Durable consent record store (YesSql document + index; retention policy; per-tenant).
- [ ] Branding polish: logo upload via media library, secondary color, dark mode, live preview
      in the admin editor.
- [ ] Locked compliance template hardening: revision handling (re-prompt on policy change),
      consent expiry (default ≤ 12 months), Do-Not-Track/GPC respect decision.
- [ ] Script/embed blocking helpers: tag helper / shape wrapper for third-party embeds
      (YouTube, Maps) with click-to-load placeholder.
- [ ] Google Consent Mode v2 signals (`ad_storage`, `analytics_storage`, `ad_user_data`,
      `ad_personalization`) wired to category state.
- [ ] German/EU defaults: German translations, TDDDG §25-appropriate default texts,
      GDPR-appropriate defaults out of the box; multi-language via OC localization.
- [ ] Cookie declaration section in the preferences modal (manually maintained table per tenant).

### Phase 2 — Quality of life (off-host scanner + exports)

- [ ] Standalone scanner service (separate repo; Azure Function/Container App + Playwright),
      explicitly off the shared host.
- [ ] Declaration ingestion API in this module (authenticated, per-tenant API key).
- [ ] Auto-generated cookie declaration page/content from scan results.
- [ ] Periodic re-scan schedule + drift alerts (new cookies found → notify admin).
- [ ] Consent log export (CSV/JSON) for audits.

### Phase 3 — Productization

- [ ] Licensing/entitlement: gate the feature per tenant/tier (integrates with the planned
      feature-profiles work on the hosting side).
- [ ] Tier definitions (e.g. banner-only vs. banner + scanning + exports).
- [ ] Release process: stable versioning, changelog, upgrade notes, demo tenant.

## Honest notes

**Ongoing compliance maintenance is a real cost, not a one-time build.** Consent-banner
requirements shift with regulator guidance (DSK), case law (Planet49 and successors), and
Google/IAB program changes (Consent Mode, TCF). Cookie categorization databases go stale.
Budget recurring time (realistically a few days per quarter) for: tracking TDDDG/GDPR/ePrivacy
developments, updating default texts and translations, updating the vendored engine, and
re-validating the locked template.

**Legal sign-off recommended.** This module implements the current mainstream technical
interpretation (prior opt-in, symmetric buttons, script blocking, consent records), but only a
lawyer can sign off that the default texts, category descriptions and record-keeping meet the
requirements for the specific sites it ships on — especially before selling it to third-party
tenants as a "compliant" product. Get a one-time legal review of the locked template and
defaults before the paid launch, and after that for material changes.
