package content.bodyPart.controller;

import content.bodyPart.controller.form.BodyPartForm;
import content.bodyPart.service.BodyPartService;
import content.common.abstracts.AbstractEditController;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("group")
class BodyPartEditController extends AbstractEditController {

    private final BodyPartService bodyPartService;
    private final BodyPartValidator bodyPartValidator;


    @Autowired
    BodyPartEditController(BodyPartService bodyPartService, BodyPartValidator bodyPartValidator) {

        Assert.notNull(bodyPartService, "bodyPartService must not be null");
        Assert.notNull(bodyPartValidator, "bodyPartValidator must not be null");

        this.bodyPartService = bodyPartService;
        this.bodyPartValidator = bodyPartValidator;
    }


    @PostMapping
    protected ResponseEntity<?> saveBodyPart(@RequestBody BodyPartForm bodyPartForm) {

        bodyPartValidator.validateBeforeSave(bodyPartForm, errors);

        return hasErrors()? errors() : response(OK, bodyPartService.saveBodyPart(bodyPartForm));
    }

    @PutMapping("/{id}")
    protected ResponseEntity<?> updateBodyPart(@PathVariable("id") UUID id,
                                               @RequestBody BodyPartForm bodyPartForm) {

        bodyPartValidator.validateBeforeUpdate(id, bodyPartForm, errors);

        return hasErrors()? errors() : response(OK, bodyPartService.updateBodyPart(id, bodyPartForm));
    }

    @DeleteMapping("/{id}")
    protected ResponseEntity<?> deleteBodyPart(@PathVariable("id") UUID id) {

        bodyPartValidator.validateBeforeDelete(id, errors);

        if (hasErrors()) return errors();

        bodyPartService.deleteBodyPart(id);

        return response(OK);
    }
}