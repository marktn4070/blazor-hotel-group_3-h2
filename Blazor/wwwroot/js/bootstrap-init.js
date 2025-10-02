// wwwroot/js/bootstrap-init.js
window.initCarousels = () => {
    if (typeof bootstrap === 'undefined') {
        console.warn('Bootstrap ikke fundet');
        return;
    }

    document.querySelectorAll('.carousel').forEach(el => {
        if (!el._bsCarousel) {
            try {
                // interval: false = ingen automatisk rotation
                el._bsCarousel = new bootstrap.Carousel(el, { interval: false, ride: false });
            } catch (e) {
                console.warn('Carousel init fejl', e);
            }
        } else {
            // ingen cycle() kald — bevarer manuel kontrol
        }
    });
};
