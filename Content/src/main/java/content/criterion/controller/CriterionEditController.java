package content.criterion.controller;

import content.common.abstracts.AbstractEditController;
import content.criterion.controller.form.CriterionForm;
import content.criterion.service.CriterionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("criterion")
class CriterionEditController extends AbstractEditController {

    private final CriterionService criterionService;
    private final CriterionValidator criterionValidator;


    @Autowired
    CriterionEditController(CriterionService criterionService, CriterionValidator criterionValidator) {

        Assert.notNull(criterionService, "criterionService must not be null");
        Assert.notNull(criterionValidator, "criterionValidator must not be null");

        this.criterionService = criterionService;
        this.criterionValidator = criterionValidator;
    }


    @PostMapping
    protected ResponseEntity<?> saveCriterion(@RequestBody CriterionForm criterionForm) {

        criterionValidator.validateBeforeSave(criterionForm, errors);

        return hasErrors()? errors() : response(OK, criterionService.saveCriterion(criterionForm));
    }

    @PutMapping("/activate/{id}")
    protected ResponseEntity<?> activateCriterion(@PathVariable("id") UUID id) {

        criterionValidator.validateBeforeActivate(id, errors);

        if (hasErrors()) return errors();

        return response(OK, criterionService.activateCriterion(id));
    }

    @PutMapping("/deactivate/{id}")
    protected ResponseEntity<?> deactivateCriterion(@PathVariable("id") UUID id) {

        criterionValidator.validateBeforeDeactivate(id, errors);

        if (hasErrors()) return errors();

        return response(OK, criterionService.deactivateCriterion(id));
    }
}