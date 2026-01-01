document.addEventListener('DOMContentLoaded', () => {
    const typingElements = document.querySelectorAll('.typing-effect');
    typingElements.forEach(element => {
        const text = element.getAttribute('data-text');
        element.style.width = '0';
        element.textContent = '';
        let i = 0;
        const type = () => {
            if (i < text.length) {
                element.textContent += text.charAt(i);
                element.style.width = `${(i + 1) * 10}px`;
                i++;
                setTimeout(type, 100);
            } else {
                element.style.borderRight = 'none';
            }
        };
        setTimeout(type, 500);
    });
});