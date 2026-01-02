// Reusable radio button validation
(function () {
    // Generic function for any radio button group
    function initializeRadioValidation(config) {
        var form = document.getElementById(config.formId);
        if (!form) return;

        function validateRadioGroup() {
            var radioYes = document.getElementById(config.radioYesId);
            var radioNo = document.getElementById(config.radioNoId);
            var errorSpan = document.getElementById(config.errorSpanId);

            if (!radioYes || !radioNo || !errorSpan) return true;

            if (!radioYes.checked && !radioNo.checked) {
                errorSpan.textContent = config.errorMessage;
                return false;
            } else {
                errorSpan.textContent = '';
                return true;
            }
        }

        function clearError() {
            var errorSpan = document.getElementById(config.errorSpanId);
            if (errorSpan) {
                errorSpan.textContent = '';
            }
        }

        // Add change event listeners
        var radioYes = document.getElementById(config.radioYesId);
        var radioNo = document.getElementById(config.radioNoId);

        if (radioYes) radioYes.addEventListener('change', clearError);
        if (radioNo) radioNo.addEventListener('change', clearError);

        // Validate on form submit
        form.addEventListener('submit', function (e) {
            var isValid = validateRadioGroup();

            if (!isValid) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        });
    }

    // Specific function for Client Evaluation (backward compatibility)
    function initializeClientEvaluationValidation(formId) {
        initializeRadioValidation({
            formId: formId,
            radioYesId: 'clientEvalYes',
            radioNoId: 'clientEvalNo',
            errorSpanId: 'clientEvalError',
            errorMessage: 'Please select whether Client Evaluation is required'
        });
    }

    // Specific function for Available For Interview
    function initializeAvailableForInterviewValidation(formId) {
        initializeRadioValidation({
            formId: formId,
            radioYesId: 'availableYes',
            radioNoId: 'availableNo',
            errorSpanId: 'availableForInterviewError',
            errorMessage: 'Please select whether available for interview'
        });
    }

    // Export to global scope
    window.initializeRadioValidation = initializeRadioValidation;
    window.initializeClientEvaluationValidation = initializeClientEvaluationValidation;
    window.initializeAvailableForInterviewValidation = initializeAvailableForInterviewValidation;
})();
