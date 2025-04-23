

document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150

    // open modals
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)

            if (modal)
                modal.style.display = 'flex';
        })
    })

    // close modals
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal-container')
            if (modal)
                modal.style.display = 'none';

            modal.querySelectorAll('form').forEach(form => {
                form.reset()

                const imagePreview = form.querySelector('.image-preview')
                if (imagePreview)
                    imagePreview.src = ''

                const imagePreviewer = form.querySelector('.image-previewer')
                if (imagePreviewer)
                    imagePreviewer.classList.remove('selected')
            })
        })
    })

    //handle image-preview
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]')
        const imagePreview = previewer.querySelector('.image-preview')

        previewer.addEventListener('click', () => fileInput.click())

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0]
            if (file)
                processImage(file, imagePreview, previewer, previewSize)
        })
    })


    //handle submit forms

    const forms = document.querySelectorAll('form')
    form.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            clearErrorMessages(form)

            const formData = new FormData(form)

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })

                if (res.ok) {
                    const modal = form.closest('.modal')
                    if (modal)
                        modal.style.display = 'none';

                    window.location.reload();
                }

                else if (!res.satus === 400) {
                    const data = await res.json()

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            let input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            let span = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (span) {
                                span.innerText = data.errors[key].join('\n');
                                span.classList.add('field-validation-error')
                            }
                        })
                    }
                }
            }
            catch {
                console.log('error submitting form.')
            }
        })
    })


    //toggle btn for notifications
    document.querySelectorAll('.project-actions').forEach(btn => {
        btn.addEventListener('click', e => {
            e.stopPropagation();
            toggleMenu(btn.getAttribute('data-menu'));
        });
    });
})

function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
    })
}

async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader()

        reader.onerror = () => reject(new Error("Failed to load file."))
        reader.onload = (e) => {
            const img = new Image()
            img.onerror = () => reject(new Error("failed to load image"))
            img.onload = () => resolve(img)
            img.src = e.target.result
        }

        reader.readAsDataURL(file)
    })
}
async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file)
        const canvas = document.createElement('canvas')
        canvas.width = previewSize
        canvas.height = previewSize

        const ctx = canvas.getContext('2d')
        ctx.drawImage(img, 0, 0, previewSize, previewSize)
        imagePreview.src = canvas.toDataURL('image/jpeg')
        previewer.classList.add('selected')
    }
    catch (error) {
        console.error('Failed on image-processing:', error)
    }
}

//toggle for signout
function toggleMenu(menuId) {
    const targetMenu = document.getElementById(menuId);
    const allMenus = document.querySelectorAll('.notification-modal-container, .profile-dropdown');

    // Stäng alla andra menyer
    allMenus.forEach(menu => {
        if (menu !== targetMenu) {
            menu.style.display = 'none';
        }
    });

    // Toggle visning på rätt meny
    if (targetMenu) {
        targetMenu.style.display = (targetMenu.style.display === 'block') ? 'none' : 'block';
    }
}

// Klick utanför = stäng alla öppna menyer
document.addEventListener('click', function (e) {
    const isClickInside = e.target.closest('.notification-modal-container') ||
        e.target.closest('#profile-avatar') ||
        e.target.closest('.notis');

    if (!isClickInside) {
        document.querySelectorAll('.notification-modal-container, .profile-dropdown')
            .forEach(menu => menu.style.display = 'none');
    }
});

// Code generated by ChatGPT-03-high
document.addEventListener('DOMContentLoaded', function () {
    var editButtons = document.querySelectorAll('.edit-member-modal');

    editButtons.forEach(function (button) {
        button.addEventListener('click', function (e) {
            e.preventDefault();


            var memberId = button.getAttribute('data-member-id');

            fetch('/Members/GetMember?id=' + encodeURIComponent(memberId))
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error('Error: ' + response.statusText);
                    }
                    return response.json();
                })
                .then(function (data) {
                    document.querySelector('#editMemberModal input[name="FirstName"]').value = data.firstName || '';
                    document.querySelector('#editMemberModal input[name="LastName"]').value = data.lastName || '';
                    document.querySelector('#editMemberModal input[name="Email"]').value = data.email || '';
                    document.querySelector('#editMemberModal input[name="PhoneNumber"]').value = data.phoneNumber || '';
                    document.querySelector('#editMemberModal input[name="JobTitle"]').value = data.jobTitle || '';
                    document.querySelector('#editMemberModal input[name="StreetName"]').value = data.streetName || '';
                    document.querySelector('#editMemberModal input[name="City"]').value = data.city || '';
                    document.querySelector('#editMemberModal input[name="PostalCode"]').value = data.postalCode || '';
                    document.querySelector('#editMemberModal input[name="Id"]').value = data.id || '';

               


                })
                .catch(function (error) {
                    console.error('Error fetching member: ', error);
                });
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    var editButtons = document.querySelectorAll('.edit-client-modal');

    editButtons.forEach(function (button) {
        button.addEventListener('click', function (e) {
            e.preventDefault();


            var clientId = button.getAttribute('data-client-id');

            fetch('/Clients/GetClient?id=' + encodeURIComponent(clientId))
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error('Error: ' + response.statusText);
                    }
                    return response.json();
                })
                .then(function (data) {
                    console.log(data);
                    document.querySelector('#editClientModal input[name="ClientName"]').value = data.clientName || '';
                    document.querySelector('#editClientModal input[name="Email"]').value = data.email || '';
                    document.querySelector('#editClientModal input[name="Location"]').value = data.location || '';
                    document.querySelector('#editClientModal input[name="PhoneNumber"]').value = data.phoneNumber || '';
                    document.querySelector('#editClientModal input[name="Id"]').value = data.id || '';




                })
                .catch(function (error) {
                    console.error('Error fetching client: ', error);
                });
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    var editButtons = document.querySelectorAll('.edit-project-modal');

    editButtons.forEach(function (button) {
        button.addEventListener('click', function (e) {
            e.preventDefault();


            var projectId = button.getAttribute('data-project-id');

            fetch('/Projects/GetProject?id=' + encodeURIComponent(projectId))
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error('Error: ' + response.statusText);
                    }
                    return response.json();
                })
                .then(function (data) {
                    console.log(data);
                    document.querySelector('#editProjectModal input[name="ProjectName"]').value = data.projectName || '';
                    document.querySelector('#editProjectModal input[name="Description"]').value = data.description || '';
                    document.querySelector('#editProjectModal input[name="Budget"]').value = data.budget || '';
                    //document.querySelector('#editProjectModal input[name="ProjectImage"]').value = data.image || '';
                    document.querySelector('#editProjectModal select[name="Members"]').value = data.projectMember.memberId || '';
                    document.querySelector('#editProjectModal select[name="ClientId"]').value = data.client.clientId || '';
                    document.querySelector('#editProjectModal input[name="StartDate"]').value = data.startDate || '';
                    document.querySelector('#editProjectModal input[name="EndDate"]').value = data.endDate || '';
                    document.querySelector('#editProjectModal select[name="StatusId"]').value = data.status.statusId || '';
                    document.querySelector('#editProjectModal input[name="Id"]').value = data.id || '';




                })
                .catch(function (error) {
                    console.error('Error fetching client: ', error);
                });
        });
    });
});

//Code generated by chatGpt

document.addEventListener('DOMContentLoaded', () => {
    // Hämta alla filter‑knappar och projekt‑kort
    const filterButtons = document.querySelectorAll('.filter-projects-by');
    const projectCards = document.querySelectorAll('.project-card');

    filterButtons.forEach(button => {
        button.addEventListener('click', e => {
            e.preventDefault();

            // 1. Rensa tidigare active
            filterButtons.forEach(b => b.classList.remove('active'));
            // 2. Markera aktuell knapp
            button.classList.add('active');

            // Läs av status ("ALL", "STARTED", "COMPLETED")
            const status = button.textContent.trim().split(' ')[0].toUpperCase();

            // Markera aktiv knapp
            filterButtons.forEach(b => b.classList.remove('active'));
            button.classList.add('active');

            // Visa eller göm kort
            projectCards.forEach(card => {
                const cardStatus = card.dataset.status; // satt i partial: data-status="Started" etc
                if (status === 'ALL' || cardStatus.toUpperCase() === status) {
                    card.style.display = '';
                } else {
                    card.style.display = 'none';
                }
            });
        });
    });

    // Triggera “ALL” vid inladdning
    if (filterButtons.length) {
        filterButtons[0].click();
    }
});


document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.wysiwyg-container').forEach(container => {
        const editorElem = container.querySelector('.wysiwyg-editor');
        const toolbarElem = container.querySelector('.wysiwyg-toolbar');
        const textarea = container.querySelector('.wysiwyg-textarea');

        // Initiera Quill på varje editor-element
        const quill = new Quill(editorElem, {
            modules: {
                syntax: true,
                toolbar: toolbarElem
            },
            placeholder: 'Type something',
            theme: 'snow'
        });

        // Sätt initialt innehåll från textarea
        if (textarea.value) {
            quill.root.innerHTML = textarea.value;
        }

        // Skriv tillbaka till textarea vid varje textändring
        quill.on('text-change', () => {
            textarea.value = quill.root.innerHTML;
        });
    });
});


document.addEventListener('DOMContentLoaded', () => {

    updateDeadline();

});
function updateDeadline() {
    const now = new Date();

    document.querySelectorAll('._time-left').forEach(el => {
        const end = new Date(el.dataset.timeLeft);
        const diffMs = end - now;
        const parent = el.closest('.deadline');

        if (diffMs < 0) {
            el.textContent = 'Expired';
            parent?.classList.remove('near');
            return;
        }

        el.textContent = formatFuture(diffMs);

        if (diffMs < 7 * 24 * 60 * 60 * 1000) {
            parent?.classList.add('near');
        } else {
            parent?.classList.remove('near');
        }
    });
}

function formatFuture(diffMs) {
    const sec = Math.floor(diffMs / 1000);
    const min = Math.floor(sec / 60);
    const hrs = Math.floor(min / 60);
    const days = Math.floor(hrs / 24);
    const weeks = Math.floor(days / 7);

    if (days < 2) return '1 day left';
    if (days < 7) return days + ' days left';
    return weeks + ' weeks left';
};