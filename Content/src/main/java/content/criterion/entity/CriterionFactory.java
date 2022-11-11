package content.criterion.entity;

import content.criterion.controller.form.CriterionForm;

public class CriterionFactory {

    public Criterion create(CriterionForm criterionForm) {

        return new Criterion(
                criterionForm.name(),
                criterionForm.unit(),
                true
        );
    }
}
