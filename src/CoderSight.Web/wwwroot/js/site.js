function csGetTurnstileToken(form) {
    var input = form.querySelector('[name="cf-turnstile-response"]');
    return input ? input.value : '';
}

function csResetTurnstile(form) {
    var widget = form.querySelector('.cf-turnstile');
    if (widget && window.turnstile) {
        var widgetId = widget.getAttribute('data-turnstile-id');
        if (widgetId) turnstile.reset(widgetId);
        else turnstile.reset();
    }
}

function csSubscribe(form) {
    var email = form.email.value;
    var msg = document.getElementById('cs-newsletter-msg');
    var btn = form.querySelector('button[type="submit"]');
    if (!email || !msg) return false;
    btn.disabled = true;
    btn.textContent = 'Subscribing...';
    var body = 'email=' + encodeURIComponent(email);
    var token = csGetTurnstileToken(form);
    if (token) body += '&cf-turnstile-response=' + encodeURIComponent(token);
    fetch('/api/newsletter/subscribe', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: body
    })
    .then(function(r) { return r.json(); })
    .then(function(data) {
        msg.textContent = data.message;
        msg.className = 'mt-3 text-sm ' + (data.success ? 'text-green-400' : 'text-red-400');
        if (data.success) form.email.value = '';
    })
    .catch(function() {
        msg.textContent = 'Something went wrong. Please try again.';
        msg.className = 'mt-3 text-sm text-red-400';
    })
    .finally(function() {
        btn.disabled = false;
        btn.textContent = form.dataset.btnText || 'Subscribe';
        csResetTurnstile(form);
    });
    return false;
}

function csContactSubmit(form) {
    var msg = form.querySelector('.cs-contact-msg');
    var btn = form.querySelector('button[type="submit"]');
    var btnText = btn.textContent;
    btn.disabled = true;
    btn.textContent = 'Sending...';
    var formData = new FormData(form);
    var body = new URLSearchParams(formData).toString();
    fetch('/api/contact', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: body
    })
    .then(function(r) { return r.json(); })
    .then(function(data) {
        if (msg) {
            msg.textContent = data.message;
            msg.className = 'cs-contact-msg text-sm ' + (data.success ? 'text-green-600' : 'text-red-600');
        }
        if (data.success) form.reset();
    })
    .catch(function() {
        if (msg) {
            msg.textContent = 'Something went wrong. Please try again.';
            msg.className = 'cs-contact-msg text-sm text-red-600';
        }
    })
    .finally(function() {
        btn.disabled = false;
        btn.textContent = btnText;
        csResetTurnstile(form);
    });
    return false;
}

function csCarouselScroll(id, dir) {
    var wrapper = document.getElementById(id);
    if (!wrapper) return;
    var track = wrapper.querySelector('.cs-carousel-track');
    if (!track) return;
    var card = track.querySelector('.cs-carousel-card');
    var scrollAmount = card ? card.offsetWidth + 24 : 324;
    track.scrollBy({ left: dir * scrollAmount, behavior: 'smooth' });
}

function csCopyUrl(url, btn) {
    navigator.clipboard.writeText(url).then(function() {
        var orig = btn.textContent;
        btn.textContent = 'Copied!';
        btn.classList.add('bg-green-500', 'text-white');
        setTimeout(function() {
            btn.textContent = orig;
            btn.classList.remove('bg-green-500', 'text-white');
        }, 1500);
    });
}

function csRegCheck() {
    var pw = document.getElementById('csRegPw');
    var pw2 = document.getElementById('csRegPwConfirm');
    var btn = document.getElementById('csRegBtn');
    if (!pw || !pw2 || !btn) return;
    var val = pw.value, val2 = pw2.value;
    var reqs = { len: val.length >= 8, upper: /[A-Z]/.test(val), lower: /[a-z]/.test(val), num: /[0-9]/.test(val) };
    var allPass = true;
    for (var k in reqs) {
        var el = document.querySelector('[data-req="' + k + '"]');
        if (el) { if (reqs[k]) el.classList.add('cs-req-pass'); else { el.classList.remove('cs-req-pass'); allPass = false; } }
    }
    var mismatch = document.getElementById('csRegMismatch');
    var match = val2.length === 0 || val === val2;
    if (mismatch) { if (!match) mismatch.classList.remove('hidden'); else mismatch.classList.add('hidden'); }
    btn.disabled = !(allPass && val === val2 && val2.length > 0);
}

function csRegValidate() {
    csRegCheck();
    var btn = document.getElementById('csRegBtn');
    return btn ? !btn.disabled : false;
}

function csRegInit() {
    var pw = document.getElementById('csRegPw');
    var pw2 = document.getElementById('csRegPwConfirm');
    if (!pw || !pw2 || pw._csRegBound) return;
    pw._csRegBound = true;
    pw.addEventListener('input', csRegCheck);
    pw2.addEventListener('input', csRegCheck);
}
function csRegDeferred() {
    csRegInit();
    setTimeout(csRegInit, 100);
    setTimeout(csRegInit, 300);
}
document.addEventListener('DOMContentLoaded', csRegDeferred);
document.addEventListener('enhancedload', csRegDeferred);

function csCopyLink(btn) {
    navigator.clipboard.writeText(window.location.href).then(function() {
        var orig = btn.textContent;
        btn.textContent = 'Copied!';
        btn.style.color = '#16a34a';
        setTimeout(function() { btn.textContent = orig; btn.style.color = ''; }, 2000);
    });
}

function csToggleMenu() {
    var menu = document.getElementById('cs-mobile-menu');
    var iconHamburger = document.getElementById('cs-icon-hamburger');
    var iconClose = document.getElementById('cs-icon-close');
    if (!menu) return;
    var isHidden = menu.classList.toggle('hidden');
    if (iconHamburger) iconHamburger.classList.toggle('hidden', !isHidden);
    if (iconClose) iconClose.classList.toggle('hidden', isHidden);
}
