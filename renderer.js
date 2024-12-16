document.getElementById('recordForm').onsubmit = async (e) => {
    e.preventDefault();

    const name = document.getElementById('name').value;
    const id = parseInt(document.getElementById('id').value, 10);

    const response = await window.electronAPI.processAction('add', { name, id });
    alert(response.message);
};

document.getElementById('sort').onclick = async () => {
    const response = await window.electronAPI.processAction('sort', {});
    alert(response.message);
};

document.getElementById('export').onclick = async () => {
    const filename = prompt('Enter filename (e.g., records.csv):');
    if (filename) {
        const response = await window.electronAPI.processAction('export', { filename });
        alert(response.message);
    }
};
