package content.criterion.service;

import content.criterion.controller.form.CriterionForm;
import content.criterion.entity.Criterion;
import content.criterion.entity.CriterionFactory;
import content.criterion.repository.CriterionRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class CriterionService {

    private final CriterionRepository criterionRepository;
    private final CriterionFactory criterionFactory;


    @Autowired
    public CriterionService(CriterionRepository criterionRepository) {

        Assert.notNull(criterionRepository, "criterionRepository must not be null");

        this.criterionRepository = criterionRepository;
        this.criterionFactory = new CriterionFactory();
    }


    public UUID saveCriterion(CriterionForm form) {

        Assert.notNull(form, "form must not be null");

        Criterion criterion = criterionFactory.create(form);

        return criterionRepository.save(criterion);
    }

    public UUID activateCriterion(UUID id) {

        Assert.notNull(id, "id must not be null");

        Criterion criterion = criterionRepository.getById(id);
        criterion.activate();

        return criterionRepository.update(criterion);
    }

    public UUID deactivateCriterion(UUID id) {

        Assert.notNull(id, "id must not be null");

        Criterion criterion = criterionRepository.getById(id);
        criterion.deactivate();

        return criterionRepository.update(criterion);
    }
}
