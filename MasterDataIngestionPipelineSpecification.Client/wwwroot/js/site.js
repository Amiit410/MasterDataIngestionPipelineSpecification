// Master Data Pipeline – site.js

// Auto-dismiss alerts after 6 seconds
document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.alert.alert-success').forEach(el => {
        setTimeout(() => {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(el);
            if (bsAlert) bsAlert.close();
        }, 6000);
    });

    // Highlight active nav link
    const path = window.location.pathname.toLowerCase();
    document.querySelectorAll('.nav-link').forEach(link => {
        const href = link.getAttribute('href');
        if (href && path.startsWith(href.toLowerCase()) && href !== '/') {
            link.classList.add('active');
        }
    });
});