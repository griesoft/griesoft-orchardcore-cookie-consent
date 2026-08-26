/**
 * Griesoft.OrchardCore.CookieConsent — banner bootstrap.
 *
 * Reads the per-tenant configuration injected by the module
 * (window.griesoftCookieConsent) and starts vanilla-cookieconsent v3.
 *
 * Compliance-critical behavior is fixed here on purpose and is NOT
 * tenant-configurable: opt-in mode, symmetric accept/reject buttons,
 * script blocking via manageScriptTags and automatic cookie clearing.
 */
(function () {
    'use strict';

    var cfg = window.griesoftCookieConsent;

    if (!cfg || typeof CookieConsent === 'undefined') {
        return;
    }

    // The "necessary" category is always present and read-only.
    var categories = {
        necessary: {
            enabled: true,
            readOnly: true
        }
    };

    var preferenceSections = [
        {
            title: 'Strictly necessary cookies',
            description: 'These cookies are essential for the site to function and cannot be disabled.',
            linkedCategory: 'necessary'
        }
    ];

    if (cfg.functionalityCategory) {
        categories.functionality = {};
        preferenceSections.push({
            title: 'Functionality cookies',
            description: 'These cookies enable enhanced functionality and personalization.',
            linkedCategory: 'functionality'
        });
    }

    if (cfg.analyticsCategory) {
        categories.analytics = {};
        preferenceSections.push({
            title: 'Analytics cookies',
            description: 'These cookies help us understand how visitors use the site.',
            linkedCategory: 'analytics'
        });
    }

    if (cfg.marketingCategory) {
        categories.marketing = {};
        preferenceSections.push({
            title: 'Marketing cookies',
            description: 'These cookies are used to deliver relevant advertisements.',
            linkedCategory: 'marketing'
        });
    }

    if (cfg.primaryColor) {
        document.documentElement.style.setProperty('--cc-btn-primary-bg', cfg.primaryColor);
        document.documentElement.style.setProperty('--cc-btn-primary-border-color', cfg.primaryColor);
        document.documentElement.style.setProperty('--cc-btn-primary-hover-bg', cfg.primaryColor);
        document.documentElement.style.setProperty('--cc-btn-primary-hover-border-color', cfg.primaryColor);
    }

    var title = cfg.bannerTitle || 'We use cookies';

    if (cfg.logoUrl) {
        var img = document.createElement('img');
        img.src = cfg.logoUrl;
        img.alt = '';
        img.style.maxHeight = '2rem';
        img.style.marginRight = '0.5rem';
        img.style.verticalAlign = 'middle';
        title = img.outerHTML + title;
    }

    // Consent-record callback stub: reports decisions to the module's endpoint.
    function recordConsent(cookie) {
        if (!cfg.recordUrl) {
            return;
        }

        var payload = JSON.stringify({
            consentId: cookie.consentId,
            acceptType: CookieConsent.getUserPreferences().acceptType,
            acceptedCategories: CookieConsent.getUserPreferences().acceptedCategories,
            rejectedCategories: CookieConsent.getUserPreferences().rejectedCategories
        });

        try {
            if (navigator.sendBeacon) {
                navigator.sendBeacon(cfg.recordUrl, new Blob([payload], { type: 'application/json' }));
            } else {
                fetch(cfg.recordUrl, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: payload,
                    keepalive: true
                });
            }
        } catch (e) {
            /* Recording consent must never break the page. */
        }
    }

    CookieConsent.run({
        // Opt-in (prior consent) mode — required under GDPR/TDDDG.
        mode: 'opt-in',

        // Block <script type="text/plain" data-category="..."> tags until opt-in
        // and clear category cookies on withdrawal.
        autoClearCookies: true,
        manageScriptTags: true,

        guiOptions: {
            consentModal: {
                layout: cfg.layout || 'box',
                position: cfg.position || 'bottom left',
                // Symmetric accept/reject — rejecting must be as easy as accepting.
                equalWeightButtons: true,
                flipButtons: false
            },
            preferencesModal: {
                layout: 'box',
                equalWeightButtons: true,
                flipButtons: false
            }
        },

        categories: categories,

        language: {
            default: 'en',
            translations: {
                en: {
                    consentModal: {
                        title: title,
                        description: cfg.bannerDescription
                            || 'We use cookies to provide essential site functionality and, with your consent, for analytics and marketing. You can accept all cookies, reject all non-essential cookies, or manage your preferences.',
                        acceptAllBtn: 'Accept all',
                        acceptNecessaryBtn: 'Reject all',
                        showPreferencesBtn: 'Manage preferences'
                    },
                    preferencesModal: {
                        title: 'Cookie preferences',
                        acceptAllBtn: 'Accept all',
                        acceptNecessaryBtn: 'Reject all',
                        savePreferencesBtn: 'Save preferences',
                        closeIconLabel: 'Close',
                        sections: preferenceSections
                    }
                }
            }
        },

        onConsent: function (param) {
            recordConsent(param.cookie);
        },

        onChange: function (param) {
            recordConsent(param.cookie);
        }
    });
})();
