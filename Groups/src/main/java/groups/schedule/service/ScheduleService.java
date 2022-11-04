package groups.schedule.service;

import groups.groupTrainer.trainer.Trainer;
import groups.groupTrainer.trainer.TrainerRepository;
import groups.schedule.dto.ScheduleCell;
import groups.schedule.dto.ScheduleCellHolder;
import groups.schedule.dto.ScheduleTrainerDto;
import groups.schedule.dto.ScheduleTrainerDtoFactory;
import groups.schedule.repository.ScheduleRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class ScheduleService {

    private final ScheduleRepository scheduleRepository;
    private final TrainerRepository trainerRepository;
    private final ScheduleTrainerDtoFactory scheduleTrainerDtoFactory;


    @Autowired
    public ScheduleService(ScheduleRepository scheduleRepository, TrainerRepository trainerRepository) {

        Assert.notNull(scheduleRepository, "scheduleRepository must not be null");
        Assert.notNull(trainerRepository, "trainerRepository must not be null");

        this.scheduleRepository = scheduleRepository;
        this.trainerRepository = trainerRepository;
        this.scheduleTrainerDtoFactory = new ScheduleTrainerDtoFactory();
    }

    public ScheduleCell composeCell(UUID groupWorkoutId) {

        ScheduleCellHolder cellHolder = scheduleRepository.getById(groupWorkoutId);

        Trainer trainer = trainerRepository.getTrainerById(cellHolder.getTrainerId());
        fillCellHolder(cellHolder, trainer);

        return cellHolder.getValidScheduleCell();
    }

    private void fillCellHolder(ScheduleCellHolder cellHolder, Trainer trainer) {

        ScheduleTrainerDto completeTrainer = scheduleTrainerDtoFactory.create(trainer);
        cellHolder.setTrainer(completeTrainer);
    }
}
