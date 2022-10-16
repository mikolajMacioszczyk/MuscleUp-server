package groups.workoutPermission.service;

import groups.workoutPermission.controller.form.WorkoutPermissionForm;
import groups.workoutPermission.entity.WorkoutPermission;
import groups.workoutPermission.entity.WorkoutPermissionFactory;
import groups.workoutPermission.repository.WorkoutPermissionRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class WorkoutPermissionService {

    private final WorkoutPermissionRepository workoutPermissionRepository;
    private final WorkoutPermissionFactory workoutPermissionFactory;


    @Autowired
    private WorkoutPermissionService(WorkoutPermissionRepository workoutPermissionRepository, WorkoutPermissionFactory workoutPermissionFactory) {

        Assert.notNull(workoutPermissionRepository, "workoutParticipantRepository must not be null");
        Assert.notNull(workoutPermissionFactory, "workoutPermissionFactory must not be null");

        this.workoutPermissionRepository = workoutPermissionRepository;
        this.workoutPermissionFactory = workoutPermissionFactory;
    }


    public UUID add(WorkoutPermissionForm workoutPermissionForm) {

        Assert.notNull(workoutPermissionForm, "workoutPermissionForm must not be null");

        WorkoutPermission workoutPermission = workoutPermissionFactory.create(workoutPermissionForm);

        return workoutPermissionRepository.add(workoutPermission);
    }

    public void remove(UUID workoutPermissionId) {

        Assert.notNull(workoutPermissionId, "workoutPermissionId must not be null");

        workoutPermissionRepository.remove(workoutPermissionId);
    }

    public void remove(WorkoutPermissionForm workoutPermissionForm) {

        Assert.notNull(workoutPermissionForm, "workoutPermissionForm must not be null");

        workoutPermissionRepository.remove(workoutPermissionForm.groupWorkoutId(), workoutPermissionForm.permissionId());
    }

    public void unassignAllByGroupWorkoutId(UUID groupWorkoutId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");

        workoutPermissionRepository.unassignAllByGroupWorkoutId(groupWorkoutId);
    }
}