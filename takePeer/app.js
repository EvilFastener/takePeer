document.getElementById('fileInput').addEventListener('change', (event) => {
    const files = event.target.files;
    const filePromises = Array.from(files).map(file => {
        if (file.name.endsWith('.json')) {
            return file.text().then(text => JSON.parse(text));
        } else {
            return Promise.reject(new Error(`Invalid file: ${file.name}`));
        }
    });

    Promise.all(filePromises)
        .then(results => {
            const container = document.getElementById('dataContainer');
            container.innerHTML = ''; 
            
            results.forEach((data, index) => {
                const fileName = files[index].name; 
                
                const fileHeader = document.createElement('div');
                fileHeader.className = 'company-header';
                fileHeader.textContent = `File: ${fileName}`;
                container.appendChild(fileHeader);

                const dataContainer = document.createElement('div');
                dataContainer.className = 'data-container';
                
                if (Array.isArray(data)) {
                    data.forEach(item => {
                        const section = document.createElement('div');
                        section.className = 'data-section';

                       
                        const toggleButton = document.createElement('button');
                        toggleButton.className = 'toggle-button';
                        toggleButton.textContent = 'Show Handshake History';
                        toggleButton.onclick = () => {
                            const historyContainer = section.querySelector('.history-container');
                            if (historyContainer.classList.contains('open')) {
                                historyContainer.classList.remove('open');
                                toggleButton.textContent = 'Show Handshake History';
                            } else {
                                historyContainer.classList.add('open');
                                toggleButton.textContent = 'Hide Handshake History';
                            }
                        };

                        section.innerHTML = `
                            <h3>ID:</h3>
                            <p>${item.Id || 'No ID available'}</p>
                            <h3>Endpoint:</h3>
                            <p>${item.Endpoint || 'No Endpoint available'}</p>
                            <h3>Latest Handshake:</h3>
                            <p>${item.latest_handshake || 'No Handshake data'}</p>
                            <h3>Transfer:</h3>
                            <p>${item.Transfer || 'No Transfer data'}</p>
                            <h3>Preshared Key:</h3>
                            <p>${item.preshared_key || 'No Preshared Key available'}</p>
                            <h3>Allowed IPs:</h3>
                            <div class="allowed-ips">
                                ${item.allowed_ips && Array.isArray(item.allowed_ips) ? 
                                    item.allowed_ips.map(ip => `<span>${ip}</span>`).join(' ') : 
                                    '<span>No allowed IPs available</span>'
                                }
                            </div>
                            <h3>Handshake History:</h3>
                            <button class="toggle-button">${item.handshake_history.length > 0 ? 'Show Handshake History' : 'No Handshake History'}</button>
                            <div class="history-container">
                                <ul>
                                    ${item.handshake_history && Array.isArray(item.handshake_history) ? 
                                        item.handshake_history.length > 0 ? 
                                            item.handshake_history.map(history => `<li>${history}</li>`).join('') :
                                            '<li>No handshake history available</li>' 
                                        : '<li>No handshake history available</li>'
                                    }
                                </ul>
                            </div>
                        `;

                        section.querySelector('.toggle-button').onclick = () => {
                            const historyContainer = section.querySelector('.history-container');
                            if (historyContainer.classList.contains('open')) {
                                historyContainer.classList.remove('open');
                                section.querySelector('.toggle-button').textContent = 'Show Handshake History';
                            } else {
                                historyContainer.classList.add('open');
                                section.querySelector('.toggle-button').textContent = 'Hide Handshake History';
                            }
                        };

                        dataContainer.appendChild(section);
                    });
                } else {
                    dataContainer.innerHTML = '<p>No data to display.</p>';
                }
                
                container.appendChild(dataContainer);
            });
        })
        .catch(error => console.error('Error processing files:', error));
});

function filterPeers() {
    const input = document.getElementById('searchInput').value.toLowerCase();
    const sections = document.querySelectorAll('.data-section');
    
    sections.forEach(section => {
        const idText = section.querySelector('p').textContent.toLowerCase();
        const endpointText = section.querySelector('p:nth-of-type(2)').textContent.toLowerCase();
        if (idText.includes(input) || endpointText.includes(input)) {
            section.style.display = '';
        } else {
            section.style.display = 'none';
        }
    });
}
