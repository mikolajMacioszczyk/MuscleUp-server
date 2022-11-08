package groups.schedule.service;

import groups.group.trainer.Trainer;
import groups.group.trainer.TrainerQuery;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.schedule.dto.ScheduleCell;
import groups.schedule.dto.ScheduleCellHolder;
import groups.schedule.dto.ScheduleTrainerDto;
import groups.schedule.dto.ScheduleTrainerDtoFactory;
import groups.schedule.repository.ScheduleRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

import static java.util.Optional.empty;
import static java.util.Optional.of;

@Service
public class ScheduleListService {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final ScheduleRepository scheduleRepository;
    private final TrainerQuery trainerQuery;
    private final ScheduleTrainerDtoFactory scheduleTrainerDtoFactory;


    @Autowired
    public ScheduleListService(GroupWorkoutQuery groupWorkoutQuery,
                               ScheduleRepository scheduleRepository,
                               TrainerQuery trainerQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(scheduleRepository, "scheduleRepository must not be null");
        Assert.notNull(trainerQuery, "trainerRepository must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.scheduleRepository = scheduleRepository;
        this.trainerQuery = trainerQuery;
        this.scheduleTrainerDtoFactory = new ScheduleTrainerDtoFactory();
    }

    public List<ScheduleCell> composeAllCells() {

        List<ScheduleCellHolder> cellHolders = scheduleRepository.getAll();

        cellHolders.forEach(this::fillTrainerData);

        return cellHolders.stream()
                .map(ScheduleCellHolder::getValidScheduleCell)
                .toList();
    }

    public List<ScheduleCell> composeCellWithClones(UUID groupWorkoutId) {

        UUID cloneId = groupWorkoutQuery.getCloneIdById(groupWorkoutId);

        List<ScheduleCellHolder> cellHolders = scheduleRepository.getWithClonesByCloneId(cloneId);

        cellHolders.forEach(this::fillTrainerData);

        return cellHolders.stream()
                .map(ScheduleCellHolder::getValidScheduleCell)
                .toList();
    }

    public ScheduleCell composeCell(UUID groupWorkoutId) {

        ScheduleCellHolder cellHolder = scheduleRepository.getById(groupWorkoutId);

        fillTrainerData(cellHolder);

        return cellHolder.getValidScheduleCell();
    }

    public Optional<ScheduleCell> tryComposeCell(UUID groupWorkoutId) {

        Optional<ScheduleCellHolder> cellHolder = scheduleRepository.findById(groupWorkoutId);

        if (cellHolder.isEmpty()) return empty();

        fillTrainerData(cellHolder.get());

        return of(cellHolder.get().getValidScheduleCell());
    }

    private void fillTrainerData(ScheduleCellHolder cellHolder) {

        Trainer trainer = trainerQuery.getTrainerById(cellHolder.getTrainerId());
        ScheduleTrainerDto completeTrainer = scheduleTrainerDtoFactory.create(trainer);
        cellHolder.setTrainer(completeTrainer);
    }
}
